using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.IocContainer;
using System.Web;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	/// <summary>
	/// 单选按钮组
	/// </summary>
	[ExportMany(ContractKey = typeof(RadioButtonsFieldAttribute)), SingletonReuse]
	public class RadioButtons : IFormFieldHandler {
		/// <summary>
		/// 构建单选按钮组的Html
		/// </summary>
		public static HtmlString BuildRadioButtonsHtml(RadioButtonsFieldAttribute attribute,
			IEnumerable<KeyValuePair<string, string>> htmlAttributes, object value) {
			var listItemProvider = (IListItemProvider)Activator.CreateInstance(attribute.Source);
			var listItems = listItemProvider.GetItems().ToList();
			var html = new HtmlTextWriter(new StringWriter());
			html.AddAttribute("class", "radio-list");
			html.RenderBeginTag("div");
			var valueString = (value is Enum) ?
				((int)value).ToString() : value.ConvertOrDefault<string>();
			foreach (var item in listItems) {
				html.AddAttribute("class", "radio-inline");
				html.RenderBeginTag("label");
				html.AddAttribute("name", attribute.Name);
				html.AddAttribute("value", item.Value);
				html.AddAttribute("type", "radio");
				if (item.Value == valueString) {
					html.AddAttribute("checked", "checked");
				}
				html.AddAttributes(htmlAttributes.Where(a => a.Key != "class"));
				html.RenderBeginTag("input");
				html.RenderEndTag(); // input
				html.WriteEncodedText(item.Name);
				html.RenderEndTag(); // label
			}
			html.RenderEndTag(); // div
			return new HtmlString(html.InnerWriter.ToString());
		}

		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var attribute = (RadioButtonsFieldAttribute)field.Attribute;
			var radioButtonsHtml = BuildRadioButtonsHtml(attribute,
				provider.FormControlAttributes.Concat(htmlAttributes), field.Value);
			return provider.FormGroupHtml(field, htmlAttributes, radioButtonsHtml.ToString());
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, string value) {
			return value;
		}
	}
}
