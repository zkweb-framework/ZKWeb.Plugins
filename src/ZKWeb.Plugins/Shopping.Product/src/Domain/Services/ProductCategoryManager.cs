using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Shopping.Product.src.Components.ExtraConfigKeys;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Entities;
using ZKWeb.Server;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Domain.Services {
	/// <summary>
	/// 商品类目管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ProductCategoryManager :
		DomainServiceBase<ProductCategory, Guid>, ICacheCleaner {
		/// <summary>
		/// 商品类目的缓存时间
		/// 默认180秒，可通过网站配置指定
		/// </summary>
		public TimeSpan CategoryCacheTime { get; set; }
		/// <summary>
		/// 类目的缓存
		/// 缓存中的类目包含属性和属性值
		/// </summary>
		protected IKeyValueCache<Guid, ProductCategory> CategoryCache { get; set; }
		/// <summary>
		/// 类目列表的缓存
		/// 缓存中的类目不包含属性和属性值
		/// </summary>
		protected IKeyValueCache<int, List<ProductCategory>> CategoryListCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductCategoryManager() {
			var configManager = Application.Ioc.Resolve<WebsiteConfigManager>();
			var cacheFactory = Application.Ioc.Resolve<ICacheFactory>();
			var extra = configManager.WebsiteConfig.Extra;
			CategoryCacheTime = TimeSpan.FromSeconds(extra.GetOrDefault(
				ProductExtraConfigKeys.ProductCategoryCacheTime, 180));
			CategoryCache = cacheFactory.CreateCache<Guid, ProductCategory>();
			CategoryListCache = cacheFactory.CreateCache<int, List<ProductCategory>>();
		}

		/// <summary>
		/// 查找类目，找不到时返回null
		/// </summary>
		/// <param name="categoryId">类目Id</param>
		/// <returns></returns>
		public virtual ProductCategory GetWithCache(Guid categoryId) {
			return CategoryCache.GetOrCreate(categoryId, () => {
				using (UnitOfWork.Scope()) {
					var category = Get(categoryId);
					// 同时获取属性信息
					if (category != null) {
						category.Properties.ToList();
						category.Properties.SelectMany(p => p.PropertyValues).ToList();
					}
					return category;
				}
			}, CategoryCacheTime);
		}

		/// <summary>
		/// 获取类目列表
		/// </summary>
		/// <returns></returns>
		public virtual IReadOnlyList<ProductCategory> GetManyWithCache() {
			return CategoryListCache.GetOrCreate(0, () => GetMany(query => {
				return query.OrderByDescending(q => q.UpdateTime).ToList();
			}), CategoryCacheTime);
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public virtual void ClearCache() {
			CategoryCache.Clear();
			CategoryListCache.Clear();
		}
	}
}
