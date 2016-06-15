using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Cache.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Shopping.Product.src.Database;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Server;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Managers {
	/// <summary>
	/// 商品类目管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ProductCategoryManager : ICacheCleaner {
		/// <summary>
		/// 商品类目的缓存时间
		/// 默认180秒，可通过网站配置指定
		/// </summary>
		public TimeSpan CategoryCacheTime { get; set; }
		/// <summary>
		/// 类目的缓存
		/// 缓存中的类目包含属性和属性值
		/// </summary>
		protected MemoryCache<long, ProductCategory> CategoryCache { get; set; }
		/// <summary>
		/// 类目列表的缓存
		/// 缓存中的类目不包含属性和属性值
		/// </summary>
		protected MemoryCache<int, List<ProductCategory>> CategoryListCache { get; set; }
		/// <summary>
		/// 属性列表的缓存
		/// 缓存中的属性不包括属性值
		/// </summary>
		protected MemoryCache<int, List<ProductProperty>> PropertyListCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductCategoryManager() {
			var configManager = Application.Ioc.Resolve<ConfigManager>();
			CategoryCacheTime = TimeSpan.FromSeconds(
				configManager.WebsiteConfig.Extra.GetOrDefault(ExtraConfigKeys.ProductCategoryCacheTime, 180));
			CategoryCache = new MemoryCache<long, ProductCategory>();
			CategoryListCache = new MemoryCache<int, List<ProductCategory>>();
			PropertyListCache = new MemoryCache<int, List<ProductProperty>>();
		}

		/// <summary>
		/// 查找类目，找不到时返回null
		/// </summary>
		/// <param name="categoryId">类目Id</param>
		/// <returns></returns>
		public virtual ProductCategory GetCategory(long categoryId) {
			// 从缓存获取
			var category = CategoryCache.GetOrDefault(categoryId);
			if (category != null) {
				return category;
			}
			// 从数据库获取
			UnitOfWork.ReadData<ProductCategory>(r => {
				category = r.GetByIdWhereNotDeleted(categoryId);
				// 同时获取属性信息，并保存到缓存
				if (category != null) {
					category.Properties.ToList();
					category.Properties.SelectMany(p => p.PropertyValues).ToList();
					CategoryCache.Put(categoryId, category, CategoryCacheTime);
				}
			});
			return category;
		}

		/// <summary>
		/// 获取类目列表
		/// </summary>
		/// <returns></returns>
		public virtual IReadOnlyList<ProductCategory> GetCategoryList() {
			// 从缓存获取
			var categoryList = CategoryListCache.GetOrDefault(0);
			if (categoryList != null) {
				return categoryList;
			}
			// 从数据库获取
			categoryList = UnitOfWork.ReadData<ProductCategory, List<ProductCategory>>(r => {
				return r.GetMany(c => !c.Deleted).ToList();
			});
			// 保存到缓存
			CategoryListCache.Put(0, categoryList, CategoryCacheTime);
			return categoryList;
		}

		/// <summary>
		/// 获取属性列表
		/// </summary>
		/// <returns></returns>
		public virtual IReadOnlyList<ProductProperty> GetPropertyList() {
			// 从缓存获取
			var propertyList = PropertyListCache.GetOrDefault(0);
			if (propertyList != null) {
				return propertyList;
			}
			// 从数据库获取
			// 按显示顺序 +更新时间排序
			propertyList = UnitOfWork.ReadData<ProductProperty, List<ProductProperty>>(r => {
				return r.GetMany(p => !p.Deleted)
					.OrderBy(p => p.DisplayOrder)
					.ThenByDescending(p => p.LastUpdated).ToList();
			});
			// 保存到缓存
			PropertyListCache.Put(0, propertyList, CategoryCacheTime);
			return propertyList;
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public virtual void ClearCache() {
			CategoryCache.Clear();
			CategoryListCache.Clear();
			PropertyListCache.Clear();
		}
	}
}
