using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWebStandard.Ioc;
using ZKWeb.Templating;
using ZKWeb.Plugins.Common.Base.src.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	/// <summary>
	/// 只读文本
	/// </summary>
	[ExportMany(ContractKey = typeof(LabelFieldAttribute)), SingletonReuse]
	public class Label : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var label = templateManager.RenderTemplate("common.base/tmpl.form.label.html", new {
				name = field.Attribute.Name,
				value = (field.Value ?? "").ToString(),
				attributes = htmlAttributes
			});
			return field.WrapFieldHtml(htmlAttributes, label);
		}

		/// <summary>
		/// 解析提交的字段的值，只读文本没有提交值
		/// </summary>
		public object Parse(FormField field, IList<string> value) {
			return null;
		}
	}
}
