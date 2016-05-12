using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugin.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Repositories;
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
		/// { (用户Id, 商品Id): 商品信息, ... }
		/// </summary>
		protected MemoryCache<Tuple<long, long>, object> ProductApiInfoCache =
			new MemoryCache<Tuple<long, long>, object>();

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
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var userId = sessionManager.GetSession().ReleatedId;
			var key = Tuple.Create(userId, productId);
			var info = ProductApiInfoCache.GetOrDefault(key);
			if (info != null) {
				return info;
			}
			// 从数据库中获取
			UnitOfWork.ReadData<Database.Product>(r => {
				var product = r.Get(p => p.Id == productId && !p.Deleted);
				if (product == null) {
					return; // 商品不存在
				}
				// 类目信息
				var productCategoryManager = Application.Ioc.Resolve<ProductCategoryManager>();
				var category = product.CategoryId == null ? null :
					productCategoryManager.FindCategory(product.CategoryId.Value);
				// 商品相册信息
				var productAlbumManager = Application.Ioc.Resolve<ProductAlbumManager>();
				var mainImageWebPath = productAlbumManager.GetAlbumImageWebPath(
					product.Id, null, ProductAlbumImageType.Normal);
				var imageWebPaths = productAlbumManager.GetExistAlbumImageWebPaths(product.Id)
					.Select(d => d.ToDictionary(p => p.Key.ToString(), p => p.Value)).ToList();
				// 卖家信息
				var seller = product.Seller;
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
					mainImageWebPath = mainImageWebPath,
					imageWebPaths = imageWebPaths
				};
				// 保存到缓存中
				ProductApiInfoCache.Put(key, info, ProductApiInfoCacheTime);
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
