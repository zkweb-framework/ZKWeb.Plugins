using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWebStandard.Ioc;
using ZKWeb.Templating;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	/// <summary>
	/// 文件上传
	/// 字段类型必须是IHttpPostedFile
	/// </summary>
	[ExportMany(ContractKey = typeof(FileUploaderFieldAttribute)), SingletonReuse]
	public class FileUploader : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var file = templateManager.RenderTemplate("common.base/tmpl.form.file.html", new {
				name = field.Attribute.Name,
				attributes = htmlAttributes
			});
			return field.WrapFieldHtml(htmlAttributes, file);
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			var attribute = (FileUploaderFieldAttribute)field.Attribute;
			var file = HttpManager.CurrentContext.Request.GetPostedFile(field.Attribute.Name);
			attribute.Check(file);
			return file;
		}
	}
}
