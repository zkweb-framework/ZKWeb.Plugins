using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.FieldFactories {
	/// <summary>
	/// 富文本编辑器生成器
	/// </summary>
	[ExportMany(ContractKey = "RichTextEditor")]
	public class RichTextEditorFactory : IDynamicFormFieldFactory {
		/// <summary>
		/// 创建表单字段属性
		/// </summary>
		public FormFieldAttribute Create(IDictionary<string, object> fieldData) {
			var name = fieldData.GetOrDefault<string>("Name");
			var config = fieldData.GetOrDefault<string>("Config");
			var imageBrowserUrl = fieldData.GetOrDefault<string>("ImageBrowserUrl");
			var group = fieldData.GetOrDefault<string>("Group");
			return new RichTextEditorAttribute(name, config) {
				ImageBrowserUrl = imageBrowserUrl, Group = group
			};
		}
	}
}
