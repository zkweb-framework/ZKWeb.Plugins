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
	[ExportMany(ContractKey = typeof(HiddenFieldAttribute)), SingletonReuse]
	public class Hidden : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		/// <param name="field"></param>
		/// <param name="htmlAttributes"></param>
		/// <returns></returns>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var html = new HtmlTextWriter(new StringWriter());
			foreach (var pair in provider.FormControlAttributes) {
				html.AddAttribute(pair.Key, pair.Value);
			}
			html.AddAttribute("name", field.Attribute.Name);
			html.AddAttribute("value", (field.Value ?? "").ToString());
			html.AddAttribute("type", "hidden");
			foreach (var pair in htmlAttributes) {
				html.AddAttribute(pair.Key, pair.Value);
			}
			html.RenderBeginTag("input");
			html.RenderEndTag();
			return html.InnerWriter.ToString();
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		/// <param name="field"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public object Parse(FormField field, string value) {
			return value;
		}
	}
}
