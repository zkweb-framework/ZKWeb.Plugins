using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.FieldFactories {
	/// <summary>
	/// 模板Html生成器
	/// </summary>
	[ExportMany(ContractKey = "TemplateHtml")]
	public class TemplateHtmlFieldFactory : IDynamicFormFieldFactory {
		/// <summary>
		/// 创建表单字段属性
		/// </summary>
		public FormFieldAttribute Create(IDictionary<string, object> fieldData) {
			var name = fieldData.GetOrDefault<string>("Name");
			var path = fieldData.GetOrDefault<string>("Path");
			var group = fieldData.GetOrDefault<string>("Group");
			return new TemplateHtmlFieldAttribute(name, path) { Group = group };
		}
	}
}
