using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;

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
		/// 解析上传的文件
		/// </summary>
		public HttpPostedFileBase Parse(HttpPostedFile file, FileUploaderFieldAttribute attribute) {
			if (file == null) {
				return null;
			} else if (!attribute.Extensions.Contains(
				Path.GetExtension(file.FileName).Substring(1))) {
				// 检查后缀
				throw new Exception(string.Format(
					new T("Only {0} files are allowed"),
					string.Join(",", attribute.Extensions)));
			} else if (file.ContentLength > attribute.MaxContentsLength) {
				// 检查大小
				throw new Exception(string.Format(
					new T("Please upload file size not greater than {0}"),
					FileUtils.GetSizeDisplayName((int)attribute.MaxContentsLength)));
			}
			return new HttpPostedFileWrapper(file);
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, string value) {
			var attribute = (FileUploaderFieldAttribute)field.Attribute;
			var file = HttpContext.Current.Request.Files[field.Attribute.Name];
			return Parse(file, attribute);
		}
	}
}
