using DryIoc;
using DryIocAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.FormFieldAttributes;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Shopping.Product.src.FormFieldHandlers {
	/// <summary>
	/// 商品属性编辑器
	/// </summary>
	[ExportMany(ContractKey = typeof(ProductPropertiesEditorAttribute)), SingletonReuse]
	public class ProductPropertiesEditor : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var attribute = (ProductPropertiesEditorAttribute)field.Attribute;
			var html = new HtmlTextWriter(new StringWriter());
			var translations = new Dictionary<string, string>() {
				{ "Sure to change category? The properties you selected will lost!",
					new T("Sure to change category? The properties you selected will lost!") }
			};
			html.AddAttribute("name", field.Attribute.Name);
			html.AddAttribute("value", JsonConvert.SerializeObject(field.Value));
			html.AddAttribute("type", "hidden");
			html.AddAttributes(provider.FormControlAttributes);
			html.AddAttributes(htmlAttributes);
			html.RenderBeginTag("input");
			html.RenderEndTag();
			html.AddAttribute("class", "product-property-editor");
			html.AddAttribute("data-toggle", "product-property-editor");
			html.AddAttribute("data-category-id-name", attribute.CategoryFieldName);
			html.AddAttribute("data-property-values-name", attribute.Name);
			html.AddAttribute("data-translations", JsonConvert.SerializeObject(translations));
			html.RenderBeginTag("div");
			html.RenderEndTag();
			return html.InnerWriter.ToString();
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, string value) {
			return JsonConvert.DeserializeObject<List<SelectedPropertyValue>>(value);
		}
	}
}
