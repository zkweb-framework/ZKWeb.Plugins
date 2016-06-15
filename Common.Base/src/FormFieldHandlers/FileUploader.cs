using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	/// <summary>
	/// 文件上传
	/// 字段类型必须是HttpPostedFileBase
	/// </summary>
	[ExportMany(ContractKey = typeof(FileUploaderFieldAttribute)), SingletonReuse]
	public class FileUploader : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var attribute = (FileUploaderFieldAttribute)field.Attribute;
			var html = new HtmlTextWriter(new StringWriter());
			html.AddAttribute("name", field.Attribute.Name);
			html.AddAttribute("class", "file-uploader");
			html.AddAttribute("type", "file");
			html.AddAttributes(provider.FormControlAttributes.Where(a => a.Key != "class"));
			html.AddAttributes(htmlAttributes);
			html.RenderBeginTag("input");
			html.RenderEndTag();
			return provider.FormGroupHtml(field, htmlAttributes, html.InnerWriter.ToString());
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, string value) {
			var attribute = (FileUploaderFieldAttribute)field.Attribute;
			var file = HttpManager.CurrentContext.Request.Files[field.Attribute.Name];
			attribute.Check(file);
			return file;
		}
	}
}
