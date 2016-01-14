using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	/// <summary>
	/// 下拉列表
	/// </summary>
	[ExportMany(ContractKey = typeof(DropdownListFieldAttribute)), SingletonReuse]
	public class DropdownList : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var attribute = (DropdownListFieldAttribute)field.Attribute;
			var listItemProvider = (IListItemProvider)Activator.CreateInstance(attribute.Source);
			var listItems = listItemProvider.GetItems().ToList();
			var html = new HtmlTextWriter(new StringWriter());
			html.AddAttribute("name", field.Attribute.Name);
			html.AddAttributes(provider.FormControlAttributes);
			html.AddAttributes(htmlAttributes);
			html.RenderBeginTag("select");
			foreach (var item in listItems) {
				html.AddAttribute("value", item.Value);
				if (item.Value == field.Value.ConvertOrDefault<string>()) {
					html.AddAttribute("selected", "selected");
				}
				html.RenderBeginTag("option");
				html.WriteEncodedText(item.Name);
				html.RenderEndTag();
			}
			html.RenderEndTag();
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
