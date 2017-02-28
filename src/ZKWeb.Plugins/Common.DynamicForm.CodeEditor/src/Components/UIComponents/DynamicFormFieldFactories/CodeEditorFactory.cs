using Newtonsoft.Json;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.CodeEditor.src.UIComponents.FormFieldAttributes;
using ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.DynamicForm.CodeEditor.src.Components.UIComponents.DynamicFormFieldFactories {
	/// <summary>
	/// 代码编辑器生成器
	/// </summary>
	[ExportMany(ContractKey = "CodeEditor")]
	public class CodeEditorFactory : IDynamicFormFieldFactory {
		/// <summary>
		/// 创建表单字段属性
		/// </summary>
		public FormFieldAttribute Create(IDictionary<string, object> fieldData) {
			var name = fieldData.GetOrDefault<string>("Name");
			var rows = fieldData.GetOrDefault<int>("Rows");
			var language = fieldData.GetOrDefault<string>("Language");
			var config = JsonConvert.SerializeObject(
				fieldData.GetOrDefault<IDictionary<string, object>>("Config") ?? (object)new { });
			var group = fieldData.GetOrDefault<string>("Group");
			return new CodeEditorAttribute(name, rows, language, config) { Group = group };
		}
	}
}
