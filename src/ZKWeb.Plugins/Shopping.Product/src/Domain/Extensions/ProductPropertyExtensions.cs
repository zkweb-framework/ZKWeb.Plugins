using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Services;
using ZKWeb.Templating;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions {
	/// <summary>
	/// 商品属性的扩展函数
	/// </summary>
	public static class ProductPropertyExtensions {
		/// <summary>
		/// 获取编辑时使用的Html
		/// </summary>
		/// <param name="property">商品属性</param>
		/// <param name="category">商品类目</param>
		/// <returns></returns>
		public static HtmlString GetEditHtml(this ProductProperty property, ProductCategory category) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var categoryManager = Application.Ioc.Resolve<ProductCategoryManager>();
			var propertyValues = property.OrderedPropertyValues().ToList();
			string templatePath = null;
			if (property.ControlType == ProductPropertyControlType.TextBox) {
				// 文本框
				templatePath = "shopping.product/property_editor.textbox.html";
			} else if (property.ControlType == ProductPropertyControlType.CheckBox) {
				// 多选框
				templatePath = "shopping.product/property_editor.checkbox.html";
			} else if (property.ControlType == ProductPropertyControlType.RadioButton) {
				// 单选按钮
				templatePath = "shopping.product/property_editor.radiobutton.html";
			} else if (property.ControlType == ProductPropertyControlType.DropdownList) {
				// 下拉框
				templatePath = "shopping.product/property_editor.dropdownlist.html";
			} else if (property.ControlType == ProductPropertyControlType.EditableDropdownList) {
				// 可编辑的下拉框
				templatePath = "shopping.product/property_editor.editable_dropdownlist.html";
			} else {
				throw new NotSupportedException(string.Format(
					"unsupported property control type {0}", property.ControlType));
			}
			return new HtmlString(templateManager.RenderTemplate(
				templatePath, new { property, propertyValues, category }));
		}

		/// <summary>
		/// 获取经过排序的属性值列表
		/// </summary>
		/// <param name="property">商品属性</param>
		/// <returns></returns>
		public static IEnumerable<ProductPropertyValue> OrderedPropertyValues(
			this ProductProperty property) {
			return property.PropertyValues.OrderBy(p => p.DisplayOrder);
		}
	}
}
