using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Components.ExtraConfigKeys;
using ZKWeb.Plugins.Shopping.Product.src.Components.GenericConfigs;
using ZKWeb.Plugins.Shopping.Product.src.Components.ProductStates.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductSalesInfoDisplayFields.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.StaticTableHandlers;
using ZKWeb.Server;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Domain.Services {
	/// <summary>
	/// 商品管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ProductManager :
		DomainServiceBase<Entities.Product, Guid>, ICacheCleaner {
		/// <summary>
		/// 商品的缓存时间
		/// 默认是3秒，可通过网站配置指定
		/// </summary>
		public TimeSpan ProductCacheTime { get; set; }
		/// <summary>
		/// 商品信息的缓存时间
		/// 默认是15秒，可通过网站配置指定
		/// </summary>
		public TimeSpan ProductApiInfoCacheTime { get; set; }
		/// <summary>
		/// 商品搜索结果的缓存时间
		/// 默认是15秒，可通过网站配置指定
		/// </summary>
		public TimeSpan ProductSearchResultCacheTime { get; set; }
		/// <summary>
		/// 商品的缓存
		/// </summary>
		protected MemoryCache<Guid, Entities.Product> ProductCache { get; set; }
		/// <summary>
		/// 商品信息的缓存
		/// </summary>
		protected IsolatedMemoryCache<Guid, object> ProductApiInfoCache { get; set; }
		/// <summary>
		/// 商品搜索结果的缓存
		/// </summary>
		protected IsolatedMemoryCache<int, StaticTableSearchResponse> ProductSearchResultCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductManager() {
			var configManager = Application.Ioc.Resolve<ConfigManager>();
			var extra = configManager.WebsiteConfig.Extra;
			ProductCacheTime = TimeSpan.FromSeconds(extra.GetOrDefault(
				ProductExtraConfigKeys.ProductCacheTime, 3));
			ProductApiInfoCacheTime = TimeSpan.FromSeconds(extra.GetOrDefault(
				ProductExtraConfigKeys.ProductApiInfoCacheTime, 15));
			ProductSearchResultCacheTime = TimeSpan.FromSeconds(extra.GetOrDefault(
				ProductExtraConfigKeys.ProductSearchResultCacheTime, 15));
			ProductCache = new MemoryCache<Guid, Entities.Product>();
			ProductApiInfoCache = new IsolatedMemoryCache<Guid, object>("Ident", "Locale");
			ProductSearchResultCache = (
				new IsolatedMemoryCache<int, StaticTableSearchResponse>("Ident", "Locale", "Url"));
		}

		/// <summary>
		/// 获取商品
		/// 商品不存在时返回null，存在时同时获取关联的数据
		/// 结果会按商品Id缓存一定时间
		/// </summary>
		/// <param name="productId"></param>
		/// <returns></returns>
		public virtual Entities.Product GetWithCache(Guid productId) {
			return ProductCache.GetOrCreate(productId, () => {
				using (UnitOfWork.Scope()) {
					var product = Get(productId);
					if (product == null) {
						return null; // 商品不存在
					}
					var category = product.Category;
					if (category != null) {
						category.Properties.ToList();
						category.Properties.SelectMany(p => p.PropertyValues).ToList();
					}
					var seller = product.Seller;
					if (seller != null) {
						var _ = seller.Username;
					}
					product.MatchedDatas.ToList();
					product.PropertyValues.ToList();
					return product;
				}
			}, ProductCacheTime);
		}

		/// <summary>
		/// 获取商品信息
		/// 商品不存在时返回null，但商品已下架或等待销售时仍然返回信息
		/// 结果会按商品Id和当前登录用户缓存一定时间
		/// </summary>
		/// <param name="productId">商品Id</param>
		/// <returns></returns>
		public virtual object GetProductApiInfo(Guid productId) {
			return ProductApiInfoCache.GetOrCreate(productId, () => {
				using (UnitOfWork.Scope()) {
					var product = Get(productId);
					if (product == null) {
						return null; // 商品不存在
					}
					// 卖家信息
					var seller = product.Seller;
					// 类目信息
					var productCategoryManager = Application.Ioc.Resolve<ProductCategoryManager>();
					var category = product.Category;
					// 相册信息
					var productAlbumManager = Application.Ioc.Resolve<ProductAlbumManager>();
					var mainImageWebPath = productAlbumManager.GetAlbumImageWebPath(
						product.Id, null, ProductAlbumImageType.Normal);
					var imageWebPaths = productAlbumManager.GetExistAlbumImageWebPaths(product.Id)
						.Select(d => d.ToDictionary(p => p.Key.ToString(), p => p.Value)).ToList();
					// 销售信息
					var salesInfoDisplayFields = Application.Ioc.ResolveMany<IProductSalesInfoDisplayField>()
						.Select(d => new { name = new T(d.Name), html = d.GetDisplayHtml(product) })
						.Where(d => !string.IsNullOrEmpty(d.html)).ToList();
					// 分类和标签
					var classes = product.Classes.Select(c => new { id = c.Id, name = c.Name }).ToList();
					var tags = product.Tags.Select(t => new { id = t.Id, name = t.Name }).ToList();
					var keywords = classes.Select(c => c.name).Concat(tags.Select(t => t.name)).ToList();
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
					// 添加时遵守原有的显示顺序
					var saleProperties = new List<object>();
					var nonSaleProperties = new List<object>();
					var groups = product.PropertyValues.GroupBy(p => p.Property.Id).Select(g => new {
						property = g.First().Property,
						propertyValues = g,
					}).OrderBy(g => g.property.DisplayOrder);
					foreach (var group in groups) {
						var obj = new {
							property = new {
								id = group.property.Id,
								name = new T(group.property.Name)
							},
							values = group.propertyValues
								.OrderBy(value =>
									value.PropertyValue == null ? 0 : value.PropertyValue.DisplayOrder)
								.Select(value => new {
									id = value.PropertyValue == null ? null : (Guid?)value.PropertyValue.Id,
									name = new T(value.PropertyValueName)
								}).ToList()
						};
						(group.property.IsSalesProperty ? saleProperties : nonSaleProperties).Add(obj);
					}
					return new {
						id = product.Id,
						categoryId = category == null ? null : (Guid?)category.Id,
						categoryName = category == null ? null : category.Name,
						name = new T(product.Name),
						introduction = product.Introduction,
						type = product.Type,
						typeName = new T(product.Type),
						state = product.State,
						stateName = new T(product.State),
						stateText = new T(string.Format("Product is {0}", product.State)),
						isPurchasable = product.GetProductState() is IAmPurchasable,
						sellerId = seller == null ? null : (Guid?)seller.Id,
						sellerName = seller == null ? null : seller.Username,
						classes,
						tags,
						keywords,
						mainImageWebPath,
						imageWebPaths,
						salesInfoDisplayFields,
						matchedDataJson,
						saleProperties,
						nonSaleProperties
					};
				}
			}, ProductApiInfoCacheTime);
		}

		/// <summary>
		/// 根据当前http请求获取搜索结果
		/// 结果会按请求参数和当前登录用户缓存一定时间
		/// </summary>
		/// <returns></returns>
		public virtual StaticTableSearchResponse GetProductSearchResponseFromHttpRequest() {
			return ProductSearchResultCache.GetOrCreate(0, () => {
				using (UnitOfWork.Scope()) {
					var configManager = Application.Ioc.Resolve<GenericConfigManager>();
					var productListSettings = configManager.GetData<ProductListSettings>();
					var searchRequest = StaticTableSearchRequest.FromHttpRequest(
						productListSettings.ProductsPerPage);
					var callbacks = new ProductTableHandler().WithExtensions();
					var searchResponse = searchRequest.BuildResponse(callbacks);
					return searchResponse;
				}
			}, ProductSearchResultCacheTime);
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public virtual void ClearCache() {
			ProductApiInfoCache.Clear();
			ProductSearchResultCache.Clear();
		}
	}
}
