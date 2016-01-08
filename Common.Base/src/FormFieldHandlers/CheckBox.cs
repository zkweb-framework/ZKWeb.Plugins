using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	/// <summary>
	/// 勾选框
	/// </summary>
	[ExportMany(ContractKey = typeof(CheckBoxFieldAttribute)), SingletonReuse]
	public class CheckBox : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var html = new HtmlTextWriter(new StringWriter());
			foreach (var pair in provider.FormControlAttributes.Where(p => p.Key != "class")) {
				html.AddAttribute(pair.Key, pair.Value);
			}
			html.AddAttribute("class", "switchery");
			html.AddAttribute("name", field.Attribute.Name);
			if (field.Value as bool? == true) {
				html.AddAttribute("checked", null);
			}
			html.AddAttribute("type", "checkbox");
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
			return value == "on";
		}
	}
}
