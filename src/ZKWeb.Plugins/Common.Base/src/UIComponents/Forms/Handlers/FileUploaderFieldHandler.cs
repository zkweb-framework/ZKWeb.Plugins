using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Templating;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Handlers {
	/// <summary>
	/// 文件上传
	/// 字段类型必须是IHttpPostedFile
	/// </summary>
	[ExportMany(ContractKey = typeof(FileUploaderFieldAttribute)), SingletonReuse]
	public class FileUploaderFieldHandler : IFormFieldHandler {
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
