using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.Database;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Shopping.Product.src.Extensions {
	/// <summary>
	/// 商品类目的扩展函数
	/// </summary>
	public static class ProductCategoryExtensions {
		/// <summary>
		/// 获取类目完整的显示名称
		/// 格式
		/// 顶级类目 > 上级类目 > 类目
		/// </summary>
		/// <param name="category">类目</param>
		/// <param name="spliter">分隔符，默认是" > "</param>
		/// <returns></returns>
		public static string GetFullName(
			this ITreeNode<ProductCategory> category, string delimiter = " > ") {
			var names = new List<string>();
			while (category.Value != null) {
				names.Add(new T(category.Value.Name));
				category = category.Parent;
			}
			return string.Join(delimiter, names.Reverse<string>());
		}

		/// <summary>
		/// 获取经过排序的属性列表
		/// </summary>
		/// <param name="category">类目</param>
		/// <returns></returns>
		public static IEnumerable<ProductProperty> OrderedProperties(
			this ProductCategory category) {
			return category.Properties.OrderBy(p => p.DisplayOrder).ThenByDescending(p => p.CreateTime);
		}
	}
}
