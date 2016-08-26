using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Validators;
using ZKWeb.Templating;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Extensions {
	/// <summary>
	/// 表单字段的扩展函数
	/// </summary>
	public static class FormFieldExtensions {
		/// <summary>
		/// 包装表单字段的Html
		/// 包装后包含: 名称，控件，验证文本
		/// </summary>
		/// <param name="field">表单字段</param>
		/// <param name="htmlAttributes">属性列表</param>
		/// <param name="fieldHtml">表单字段的Html</param>
		/// <returns></returns>
		public static string WrapFieldHtml(
			this FormField field, IDictionary<string, string> htmlAttributes, string fieldHtml) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return templateManager.RenderTemplate("common.base/tmpl.form.field_wrapper.html", new {
				name = field.Attribute.Name,
				required = htmlAttributes.ContainsKey(RequiredValidator.Key),
				attributes = htmlAttributes,
				fieldHtml = new HtmlString(fieldHtml)
			});
		}
	}
}
