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
	public static class ProductToPropertyValueExtensions {
		/// <summary>
		/// 转换到编辑使用的列表
		/// </summary>
		/// <param name="values">数据库中的商品属性值列表</param>
		/// <returns></returns>
		public static List<ProductToPropertyValueForEdit> ToEditList(
			this ISet<ProductToPropertyValue> values) {
			if (values == null) {
				return null;
			}
			return values.Select(v => new ProductToPropertyValueForEdit() {
				propertyId = v.Property.Id,
				propertyValueId = v.PropertyValue.Id,
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
			this List<ProductToPropertyValueForEdit> values, Database.Product product) {
			if (values == null || product.Category == null) {
				return new HashSet<ProductToPropertyValue>();
			}
			return new HashSet<ProductToPropertyValue>(values.Select(value => {
				var property = product.Category.Properties.First(p => p.Id == value.propertyId);
				var propertyValue = property.PropertyValues.FirstOrDefault(p => p.Id == value.propertyValueId);
				return new ProductToPropertyValue() {
					Product = product,
					Property = property,
					PropertyValue = propertyValue,
					PropertyValueName = value.name
				};
			}));
		}
	}
}
