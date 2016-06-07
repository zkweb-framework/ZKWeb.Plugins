using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Shopping.Product.src.FormFieldAttributes;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.IocContainer;

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
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var attribute = (ProductPropertyValuesEditorAttribute)field.Attribute;
			var html = new HtmlTextWriter(new StringWriter());
			var translations = new Dictionary<string, string>() {
				{ "Name", new T("Name") },
				{ "Remark", new T("Remark") }
			};
			html.AddAttribute("name", field.Attribute.Name);
			html.AddAttribute("value", JsonConvert.SerializeObject(field.Value));
			html.AddAttribute("type", "hidden");
			html.AddAttributes(provider.FormControlAttributes);
			html.AddAttributes(htmlAttributes);
			html.RenderBeginTag("input");
			html.RenderEndTag();
			html.AddAttribute("class", "product-property-values-editor");
			html.AddAttribute("data-toggle", "product-property-values-editor");
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
			return JsonConvert.DeserializeObject<List<ProductPropertyValueForEdit>>(value) ??
				new List<ProductPropertyValueForEdit>();
		}
	}
}
