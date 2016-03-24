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
			// FIXME: 进行安全处理
			return value;
		}
	}
}
