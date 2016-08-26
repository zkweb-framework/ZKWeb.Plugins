using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Interfaces;
using ZKWeb.Templating;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Handlers {
	/// <summary>
	/// 勾选框
	/// </summary>
	[ExportMany(ContractKey = typeof(CheckBoxFieldAttribute)), SingletonReuse]
	public class CheckBoxFieldHandler : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			if (field.Value.ConvertOrDefault<bool?>() == true) {
				htmlAttributes["checked"] = "checked";
			} else {
				htmlAttributes.Remove("checked");
			}
			var checkbox = templateManager.RenderTemplate("common.base/tmpl.form.checkbox.html", new {
				name = field.Attribute.Name,
				attributes = htmlAttributes
			});
			return field.WrapFieldHtml(htmlAttributes, checkbox);
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			return values[0] == "on";
		}
	}
}
