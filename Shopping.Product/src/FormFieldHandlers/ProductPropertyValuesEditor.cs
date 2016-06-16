using Newtonsoft.Json;
using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.FormFieldAttributes;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Templating;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.FormFieldHandlers {
	/// <summary>
	/// 商品属性值的编辑器
	/// 编辑商品属性时使用
	/// </summary>
	[ExportMany(ContractKey = typeof(ProductPropertyValuesEditorAttribute)), SingletonReuse]
	public class ProductPropertyValuesEditor : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var attribute = (ProductPropertyValuesEditorAttribute)field.Attribute;
			var translations = new Dictionary<string, string>() {
				{ "Name", new T("Name") },
				{ "Remark", new T("Remark") }
			};
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return templateManager.RenderTemplate(
				"shopping.product/tmpl.form.product_property_values_editor.html", new {
					name = attribute.Name,
					value = JsonConvert.SerializeObject(field.Value),
					attributes = htmlAttributes,
					translations = JsonConvert.SerializeObject(translations)
				});
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			return JsonConvert.DeserializeObject<List<ProductPropertyValueForEdit>>(values[0]) ??
				new List<ProductPropertyValueForEdit>();
		}
	}
}
