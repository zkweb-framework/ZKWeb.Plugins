using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWebStandard.Ioc;
using ZKWeb.Templating;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	/// <summary>
	/// 隐藏字段
	/// </summary>
	[ExportMany(ContractKey = typeof(HiddenFieldAttribute)), SingletonReuse]
	public class Hidden : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return templateManager.RenderTemplate("common.base/tmpl.form.hidden.html", new {
				name = field.Attribute.Name,
				value = (field.Value ?? "").ToString(),
				attributes = htmlAttributes
			});
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			return values[0];
		}
	}
}
