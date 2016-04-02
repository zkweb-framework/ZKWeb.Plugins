using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Templating;

namespace ZKWeb.Plugins.Shopping.Product.src.Extensions {
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
			var propertyValues = categoryManager.GetPropertyValues(property).ToList();
			string templatePath = null;
			if (property.ControlType == ProductPropertyControlType.TextBox) {
				// 文本框
				templatePath = "shopping.product/property_editor.textbox.html";
			} else if (property.ControlType == ProductPropertyControlType.CheckBox) {
				// 多选框
				templatePath = "shopping.product/property_editor.checkbox.html";
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
	}
}
