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
	/// 商品相册管理器
	/// </summary>
	[ExportMany]
	public class ProductPropertyManager :
		DomainServiceBase<ProductProperty, Guid>, ICacheCleaner {
		/// <summary>
		/// 商品类目的缓存时间
		/// 默认180秒，可通过网站配置指定
		/// </summary>
		public TimeSpan CategoryCacheTime { get; set; }
		/// <summary>
		/// 属性列表的缓存
		/// 缓存中的属性不包括属性值
		/// </summary>
		protected IKeyValueCache<int, List<ProductProperty>> PropertyListCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductPropertyManager() {
			var configManager = Application.Ioc.Resolve<WebsiteConfigManager>();
			var cacheFactory = Application.Ioc.Resolve<ICacheFactory>();
			var extra = configManager.WebsiteConfig.Extra;
			CategoryCacheTime = TimeSpan.FromSeconds(extra.GetOrDefault(
				ProductExtraConfigKeys.ProductCategoryCacheTime, 180));
			PropertyListCache = cacheFactory.CreateCache<int, List<ProductProperty>>();
		}

		/// <summary>
		/// 获取属性列表
		/// </summary>
		/// <returns></returns>
		public virtual IReadOnlyList<ProductProperty> GetManyWithCache() {
			return PropertyListCache.GetOrCreate(0, () => GetMany(query => {
				return query.OrderBy(p => p.DisplayOrder)
					.ThenByDescending(p => p.UpdateTime).ToList();
			}), CategoryCacheTime);
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public virtual void ClearCache() {
			PropertyListCache.Clear();
		}
	}
}

