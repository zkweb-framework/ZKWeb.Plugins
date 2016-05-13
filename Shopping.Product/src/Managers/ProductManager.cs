using DryIoc;
using DryIocAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugin.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Collections;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Utils.Collections;

namespace ZKWeb.Plugins.Shopping.Product.src.Managers {
	/// <summary>
	/// 商品管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ProductManager : ICacheCleaner {
		/// <summary>
		/// Api使用的商品信息的缓存时间，默认是15秒
		/// </summary>
		public TimeSpan ProductApiInfoCacheTime { get; set; }
		/// <summary>
		/// Api使用的商品信息的缓存
		/// </summary>
		protected MemoryCacheByIdentityAndLocale<long, object> ProductApiInfoCache =
			new MemoryCacheByIdentityAndLocale<long, object>();

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductManager() {
			ProductApiInfoCacheTime = TimeSpan.FromSeconds(15);
		}

		/// <summary>
		/// 获取Api使用的商品信息
		/// 商品不存在时返回null，但商品已下架或等待销售时仍然返回信息
		/// </summary>
		/// <param name="productId">商品Id</param>
		/// <returns></returns>
		public virtual object GetProductApiInfo(long productId) {
			// 从缓存中获取
			var info = ProductApiInfoCache.GetOrDefault(productId);
			if (info != null) {
				return info;
			}
			// 从数据库中获取
			UnitOfWork.ReadData<Database.Product>(r => {
				var product = r.Get(p => p.Id == productId && !p.Deleted);
				if (product == null) {
					return; // 商品不存在
				}
				// 卖家信息
				var seller = product.Seller;
				// 类目信息
				var productCategoryManager = Application.Ioc.Resolve<ProductCategoryManager>();
				var category = product.CategoryId == null ? null :
					productCategoryManager.FindCategory(product.CategoryId.Value);
				// 相册信息
				var productAlbumManager = Application.Ioc.Resolve<ProductAlbumManager>();
				var mainImageWebPath = productAlbumManager.GetAlbumImageWebPath(
					product.Id, null, ProductAlbumImageType.Normal);
				var imageWebPaths = productAlbumManager.GetExistAlbumImageWebPaths(product.Id)
					.Select(d => d.ToDictionary(p => p.Key.ToString(), p => p.Value)).ToList();
				// 销售信息
				var salesInfoDisplayFields = Application.Ioc.ResolveMany<IProductSalesInfoDisplayField>()
					.Select(d => new { name = new T(d.Name), html = d.GetDisplayHtml(r.Context, product) })
					.Where(d => !string.IsNullOrEmpty(d.html)).ToList();
				// 匹配数据
				var matchedDataJson = JsonConvert.SerializeObject(
					product.MatchedDatas.Select(d => new {
						Conditions = d.Conditions,
						Affects = d.Affects,
						Price = d.Price,
						PriceCurrency = d.PriceCurrency,
						PriceCurrencyInfo = d.GetCurrency(),
						Weight = d.Weight,
						Stock = d.Stock,
						MatchOrder = d.MatchOrder
					}).OrderBy(d => d.MatchOrder));
				// 销售和非销售属性
				var saleProperties = new List<object>();
				var nonSaleProperties = new List<object>();
				foreach (var group in product.PropertyValues.GroupBy(p => p.PropertyId)) {
					var property = productCategoryManager.FindProperty(product.CategoryId ?? 0, group.Key);
					if (property == null) {
						continue;
					}
					var obj = new {
						property = new { id = property.Id, name = new T(property.Name) },
						values = group.Select(e => new {
							id = e.PropertyValueId,
							name = new T(e.PropertyValueName)
						}).ToList()
					};
					(property.IsSaleProperty ? saleProperties : nonSaleProperties).Add(obj);
				}
				info = new {
					id = product.Id,
					categoryId = product.CategoryId,
					categoryName = category == null ? null : category.Name,
					name = product.Name,
					type = product.Type,
					typeName = new T(product.Type),
					state = product.State,
					stateName = new T(product.State),
					sellerId = seller == null ? null : (long?)seller.Id,
					sellerName = seller == null ? null : seller.Username,
					classes = product.Classes.Select(c => new { id = c.Id, name = c.Name }).ToList(),
					tags = product.Tags.Select(t => new { id = t.Id, name = t.Name }).ToList(),
					mainImageWebPath,
					imageWebPaths,
					salesInfoDisplayFields,
					matchedDataJson,
					saleProperties,
					nonSaleProperties
				};
				// 保存到缓存中
				ProductApiInfoCache.Put(productId, info, ProductApiInfoCacheTime);
			});
			return info;
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public virtual void ClearCache() {
			ProductApiInfoCache.Clear();
		}
	}
}
