using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Product.src.Model {
	/// <summary>
	/// 商品类目提供器
	/// </summary>
	public interface IProductCategoryProvider {
		/// <summary>
		/// 获取类目列表
		/// </summary>
		/// <returns></returns>
		IEnumerable<ProductCategory> GetCategories();
		/// <summary>
		/// 获取属性列表
		/// </summary>
		/// <returns></returns>
		IEnumerable<ProductProperty> GetProperties();
		/// <summary>
		/// 获取属性值列表
		/// </summary>
		/// <returns></returns>
		IEnumerable<ProductPropertyValue> GetPropertyValues();
	}
}
