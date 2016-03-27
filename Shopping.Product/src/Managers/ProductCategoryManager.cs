using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugin.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Utils.Collections;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Shopping.Product.src.Managers {
	/// <summary>
	/// 商品类目管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ProductCategoryManager : ICacheCleaner {
		/// <summary>
		/// 类目Id到类目的索引
		/// </summary>
		protected LazyCache<Dictionary<long, ProductCategory>> CategoryMapping = null;
		/// <summary>
		/// 类目树
		/// </summary>
		protected LazyCache<ITreeNode<ProductCategory>> CategoryTree = null;
		/// <summary>
		/// 属性Id到属性列表的索引
		/// </summary>
		protected LazyCache<Dictionary<long, List<ProductProperty>>> PropertyMapping = null;
		/// <summary>
		/// 属性值Id到属性值的索引
		/// </summary>
		protected LazyCache<Dictionary<long, ProductPropertyValue>> ProductPropertyValueMapping = null;

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductCategoryManager() {
			CategoryMapping = LazyCache.Create(() => {
				return Application.Ioc.ResolveMany<IProductCategoryProvider>()
					.SelectMany(p => p.GetCategories())
					.ToDictionary(c => c.Id);
			});
			CategoryTree = LazyCache.Create(() => {
				return TreeUtils.CreateTree(CategoryMapping.Value.Values,
					c => c, c => CategoryMapping.Value.GetOrDefault(c.ParentId));
			});
			PropertyMapping = LazyCache.Create(() => {
				return Application.Ioc.ResolveMany<IProductCategoryProvider>()
					.SelectMany(p => p.GetProperties())
					.GroupBy(p => p.Id)
					.ToDictionary(g => g.Key, g => g.ToList());
			});
			ProductPropertyValueMapping = LazyCache.Create(() => {
				return Application.Ioc.ResolveMany<IProductCategoryProvider>()
					.SelectMany(p => p.GetPropertyValues())
					.ToDictionary(v => v.Id);
			});
		}

		/// <summary>
		/// 查找类目Id对应的类目，找不到时返回null
		/// </summary>
		/// <param name="categoryId">类目Id</param>
		/// <returns></returns>
		public virtual ProductCategory FindCategory(long categoryId) {
			return CategoryMapping.Value.GetOrDefault(categoryId);
		}

		/// <summary>
		/// 查找类目Id和属性Id对应的属性，找不到时返回null
		/// </summary>
		/// <param name="categoryId">类目Id</param>
		/// <param name="propertyId">属性Id</param>
		/// <returns></returns>
		public virtual ProductProperty FindProperty(long categoryId, long propertyId) {
			var properties = PropertyMapping.Value.GetOrDefault(propertyId);
			if (properties == null) {
				return null;
			}
			return properties.FirstOrDefault(p =>
				p.ParentCategoryIds != null && p.ParentCategoryIds.Contains(categoryId));
		}

		/// <summary>
		/// 获取所有包含类目的树
		/// </summary>
		/// <returns></returns>
		public virtual ITreeNode<ProductCategory> GetCategoryTree() {
			return CategoryTree.Value;
		}

		/// <summary>
		/// 获取类目下的属性列表
		/// </summary>
		/// <param name="category">类目</param>
		/// <returns></returns>
		public virtual IEnumerable<ProductProperty> GetProperties(ProductCategory category) {
			return category.PropertyIds
				.Select(id => FindProperty(category.Id, id))
				.Where(p => p != null);
		}

		/// <summary>
		/// 获取属性下的值列表
		/// </summary>
		/// <param name="property">属性</param>
		/// <returns></returns>
		public virtual IEnumerable<ProductPropertyValue> GetPropertyValues(ProductProperty property) {
			return property.PropertyValueIds
				.Select(id => ProductPropertyValueMapping.Value.GetOrDefault(id))
				.Where(v => v != null);
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public void ClearCache() {
			CategoryMapping.Reset();
			CategoryTree.Reset();
			PropertyMapping.Reset();
			ProductPropertyValueMapping.Reset();
		}
	}
}
