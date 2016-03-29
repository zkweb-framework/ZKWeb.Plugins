using DryIoc;
using DryIocAttributes;
using Ganss.XSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.UI.CKEditor.src.FormFieldAttributes;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.UI.CKEditor.src.FormFieldHandlers {
	/// <summary>
	/// CKEditor编辑器
	/// </summary>
	[ExportMany(ContractKey = typeof(CKEditorAttribute)), SingletonReuse]
	public class CKEditor : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var attribute = (CKEditorAttribute)field.Attribute;
			var html = new HtmlTextWriter(new StringWriter());
			html.AddAttribute("name", field.Attribute.Name);
			html.AddAttribute("require-script", "/static/ui.ckeditor.js/ckeditor-loader.min.js");
			html.AddAttribute("class", "form-control ckeditor");
			html.AddAttribute("data-ckeditor-config", attribute.Config);
			html.AddAttributes(provider.FormControlAttributes.Where(a => a.Key != "class"));
			html.AddAttributes(htmlAttributes);
			html.RenderBeginTag("textarea");
			html.WriteEncodedText((field.Value ?? "").ToString());
			html.RenderEndTag();
			return provider.FormGroupHtml(field, htmlAttributes, html.InnerWriter.ToString());
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, string value) {
			// 解码提交回来的Html内容
			var html = HttpUtility.HtmlDecode(value);
			// 过滤不安全的内容
			var sanitizer = new HtmlSanitizer();
			sanitizer.RemovingAttribute += (e, args) => {
				// 允许base64图片，因为Uri有65520的长度限制，只能使用事件允许
				if (args.Attribute.Key == "src" && args.Attribute.Value.StartsWith("data:")) {
					args.Cancel = true;
				}
			};
			html = sanitizer.Sanitize(html);
			return html;
		}
	}
}
