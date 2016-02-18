using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	/// <summary>
	/// 多行文本框
	/// </summary>
	[ExportMany(ContractKey = typeof(TextAreaFieldAttribute)), SingletonReuse]
	public class TextArea : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		/// <param name="field"></param>
		/// <param name="htmlAttributes"></param>
		/// <returns></returns>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var attribute = (TextAreaFieldAttribute)field.Attribute;
			var html = new HtmlTextWriter(new StringWriter());
			html.AddAttribute("name", field.Attribute.Name);
			html.AddAttribute("rows", attribute.Rows.ToString());
			html.AddAttribute("placeholder", new T(attribute.PlaceHolder));
			html.AddAttributes(provider.FormControlAttributes);
			html.AddAttributes(htmlAttributes);
			html.RenderBeginTag("textarea");
			html.WriteEncodedText((field.Value ?? "").ToString());
			html.RenderEndTag();
			return provider.FormGroupHtml(field, htmlAttributes, html.InnerWriter.ToString());
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
