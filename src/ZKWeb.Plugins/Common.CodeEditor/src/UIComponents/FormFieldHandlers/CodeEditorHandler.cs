using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.CodeEditor.src.UIComponents.FormFieldAttributes;
using ZKWeb.Templating;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CodeEditor.src.UIComponents.FormFieldHandlers {
	/// <summary>
	/// 代码编辑器的字段处理器
	/// </summary>
	[ExportMany(ContractKey = typeof(CodeEditorAttribute)), SingletonReuse]
	public class CodeEditorHandler : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var attribute = (CodeEditorAttribute)field.Attribute;
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var textarea = templateManager.RenderTemplate(
				"common.codeeditor/tmpl.form.codeeditor.html", new {
					name = attribute.Name,
					rows = attribute.Rows,
					value = (field.Value ?? "").ToString(),
					placeholder = new T(attribute.PlaceHolder),
					language = attribute.Language,
					config = attribute.Config,
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
