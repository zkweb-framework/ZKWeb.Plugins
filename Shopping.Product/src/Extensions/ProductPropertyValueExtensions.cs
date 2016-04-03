using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Shopping.Product.src.Database;
using ZKWeb.Plugins.Shopping.Product.src.Model;

namespace ZKWeb.Plugins.Shopping.Product.src.Extensions {
	/// <summary>
	/// 商品属性值的扩展函数
	/// </summary>
	public static class ProductPropertyValueExtensions {
		/// <summary>
		/// 转换到编辑使用的列表
		/// </summary>
		/// <param name="values">数据库中的商品属性值列表</param>
		/// <returns></returns>
		public static List<EditingPropertyValue> ToEditList(this ISet<ProductToPropertyValue> values) {
			if (values == null) {
				return null;
			}
			return values.Select(v => new EditingPropertyValue() {
				propertyId = v.PropertyId,
				propertyValueId = v.PropertyValueId,
				name = v.PropertyValueName
			}).ToList();
		}

		/// <summary>
		/// 转换到数据库使用的集合
		/// </summary>
		/// <param name="values">编辑后的商品属性值列表</param>
		/// <param name="product">商品</param>
		/// <returns></returns>
		public static ISet<ProductToPropertyValue> ToDatabaseSet(
			this List<EditingPropertyValue> values, Database.Product product) {
			if (values == null || product.CategoryId == null) {
				return new HashSet<ProductToPropertyValue>();
			}
			return new HashSet<ProductToPropertyValue>(values.Select(v => new ProductToPropertyValue() {
				Product = product,
				CategoryId = product.CategoryId.Value,
				PropertyId = v.propertyId,
				PropertyValueId = v.propertyValueId,
				PropertyValueName = v.name
			}));
		}
	}
}
