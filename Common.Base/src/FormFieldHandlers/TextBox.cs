using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src.FormFieldValidators;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	/// <summary>
	/// 文本框
	/// </summary>
	[ExportMany(ContractKey = typeof(TextBoxFieldAttribute)), SingletonReuse]
	public class TextBox : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var attribute = (TextBoxFieldAttribute)field.Attribute;
			var html = new HtmlTextWriter(new StringWriter());
			foreach (var pair in provider.FormControlAttributes) {
				html.AddAttribute(pair.Key, pair.Value);
			}
			html.AddAttribute("name", field.Attribute.Name);
			html.AddAttribute("value", (field.Value ?? "").ToString());
			html.AddAttribute("type", "text");
			html.AddAttribute("placeholder", new T(attribute.PlaceHolder));
			foreach (var pair in htmlAttributes) {
				html.AddAttribute(pair.Key, pair.Value);
			}
			html.RenderBeginTag("input");
			html.RenderEndTag();
			return provider.FormGroupHtml(
				field, htmlAttributes, html.InnerWriter.ToString());
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, string value) {
			return value;
		}
	}
}
