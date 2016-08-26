using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Interfaces;
using ZKWeb.Templating;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Handlers {
	/// <summary>
	/// 多行文本框
	/// </summary>
	[ExportMany(ContractKey = typeof(TextAreaFieldAttribute)), SingletonReuse]
	public class TextAreaFieldHandler : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var attribute = (TextAreaFieldAttribute)field.Attribute;
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var textarea = templateManager.RenderTemplate("common.base/tmpl.form.textarea.html", new {
				name = attribute.Name,
				rows = attribute.Rows,
				value = (field.Value ?? "").ToString(),
				placeholder = new T(attribute.PlaceHolder),
				attributes = htmlAttributes
			});
			return field.WrapFieldHtml(htmlAttributes, textarea);
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			return values[0];
		}
	}
}
