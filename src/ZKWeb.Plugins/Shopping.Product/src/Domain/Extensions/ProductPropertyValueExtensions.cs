using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions {
	/// <summary>
	/// 商品属性值的扩展函数
	/// </summary>
	public static class ProductPropertyValueExtensions {
		/// <summary>
		/// 转换到编辑使用的列表
		/// </summary>
		/// <param name="values">数据库中的商品属性对应的属性值列表</param>
		/// <returns></returns>
		public static IList<ProductPropertyValueForEdit> ToEditList(
			this ISet<ProductPropertyValue> values) {
			if (values == null) {
				return null;
			}
			return values.OrderBy(v => v.DisplayOrder)
				.Select(v => new ProductPropertyValueForEdit() {
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
			this IList<ProductPropertyValueForEdit> values, ProductProperty property) {
			if (values == null) {
				return new HashSet<ProductPropertyValue>();
			}
			var set = property.PropertyValues ?? new HashSet<ProductPropertyValue>();
			var addValues = new List<ProductPropertyValue>();
			var now = DateTime.UtcNow;
			for (int i = 0; i < values.Count; ++i) {
				var value = values[i];
				if (value.Id == null) {
					// 添加属性值（临时）
					addValues.Add(new ProductPropertyValue() {
						Id = GuidUtils.SequentialGuid(now),
						Name = value.Name,
						Property = property,
						DisplayOrder = i,
						CreateTime = now,
						UpdateTime = now,
						Remark = value.Remark
					});
				} else {
					// 更新属性值
					var updateValue = set.First(p => p.Id == value.Id);
					updateValue.Name = value.Name;
					updateValue.DisplayOrder = i;
					updateValue.UpdateTime = now;
					updateValue.Remark = value.Remark;
				}
			}
			// 删除属性值
			var aliveIds = new HashSet<Guid>(values.Where(v => v.Id != null).Select(v => v.Id.Value));
			var deleteValues = set.Where(p => !aliveIds.Contains(p.Id)).ToList();
			deleteValues.ForEach(v => set.Remove(v));
			// 添加属性值
			addValues.ForEach(v => set.Add(v));
			return set;
		}
	}
}
