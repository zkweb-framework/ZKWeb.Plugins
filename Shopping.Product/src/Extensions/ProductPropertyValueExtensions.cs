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
		/// <param name="values">数据库中的商品属性对应的属性值列表</param>
		/// <returns></returns>
		public static List<ProductPropertyValueForEdit> ToEditList(
			this ISet<ProductPropertyValue> values) {
			if (values == null) {
				return null;
			}
			return values.Select(v => new ProductPropertyValueForEdit() {
				Id = v.Id,
				Name = v.Name,
				Remark = v.Remark
			}).ToList();
		}

		/// <summary>
		/// 转换到数据库使用的集合
		/// </summary>
		/// <param name="values">编辑后的商品属性对应的属性值列表</param>
		/// <param name="property">商品属性</param>
		/// <returns></returns>
		public static ISet<ProductPropertyValue> ToDatabaseSet(
			this List<ProductPropertyValueForEdit> values, ProductProperty property) {
			if (values == null) {
				return new HashSet<ProductPropertyValue>();
			}
			var set = property.PropertyValues ?? new HashSet<ProductPropertyValue>();
			for (int i = 0; i < values.Count; ++i) {
				var value = values[i];
				if (value.Id == null) {
					// 添加属性值
					set.Add(new ProductPropertyValue() {
						Name = value.Name,
						Property = property,
						DisplayOrder = i,
						CreateTime = DateTime.UtcNow,
						LastUpdated = DateTime.UtcNow,
						Remark = value.Remark
					});
				} else {
					// 更新属性值
					var updateValue = set.First(p => p.Id == value.Id);
					updateValue.Name = value.Name;
					updateValue.DisplayOrder = i;
					updateValue.LastUpdated = DateTime.UtcNow;
					updateValue.Remark = value.Remark;
				}
			}
			// 删除属性值
			var aliveIds = new HashSet<long>(values.Where(v => v.Id != null).Select(v => v.Id.Value));
			var deleteValues = set.Where(p => p.Id > 0 && !aliveIds.Contains(p.Id)).ToList();
			deleteValues.ForEach(v => set.Remove(v));
			return set;
		}
	}
}
