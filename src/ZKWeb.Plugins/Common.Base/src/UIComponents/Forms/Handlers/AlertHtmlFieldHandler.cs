using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Templating;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Handlers {
	/// <summary>
	/// 显示提示的Html处理器
	/// </summary>
	[ExportMany(ContractKey = typeof(AlertHtmlFieldAttribute)), SingletonReuse]
	public class AlertHtmlFieldHandler : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var attribute = (AlertHtmlFieldAttribute)field.Attribute;
			if (field.Value != null && field.Value.ToString() != "") {
				var templateManager = Application.Ioc.Resolve<TemplateManager>();
				return templateManager.RenderTemplate(
					"common.base/tmpl.form.alert.html",
					new { type = attribute.Type, message = field.Value });
			}
			return null;
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			return null;
		}
	}
}
