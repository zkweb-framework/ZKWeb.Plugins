using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	/// <summary>
	/// 单选按钮组
	/// </summary>
	[ExportMany(ContractKey = typeof(RadioButtonsFieldAttribute)), SingletonReuse]
	public class RadioButtons : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var attribute = (RadioButtonsFieldAttribute)field.Attribute;
			var listItemProvider = (IListItemProvider)Activator.CreateInstance(attribute.Source);
			var listItems = listItemProvider.GetItems().ToList();
			var html = new HtmlTextWriter(new StringWriter());
			html.AddAttribute("class", "radio-list");
			html.RenderBeginTag("div");
			foreach (var item in listItems) {
				html.AddAttribute("class", "radio-inline");
				html.RenderBeginTag("label");
				html.AddAttribute("name", field.Attribute.Name);
				html.AddAttribute("value", item.Value);
				html.AddAttribute("type", "radio");
				if (item.Value == field.Value.ConvertOrDefault<string>()) {
					html.AddAttribute("checked", "checked");
				}
				html.AddAttributes(provider.FormControlAttributes.Where(a => a.Key != "class"));
				html.AddAttributes(htmlAttributes);
				html.RenderBeginTag("input");
				html.RenderEndTag(); // input
				html.WriteEncodedText(item.Name);
				html.RenderEndTag(); // label
			}
			html.RenderEndTag(); // div
			return provider.FormGroupHtml(field, htmlAttributes, html.InnerWriter.ToString());
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, string value) {
			return value;
		}
	}
}
