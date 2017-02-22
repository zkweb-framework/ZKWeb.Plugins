using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.FieldFactories {
	/// <summary>
	/// 文本框生成器
	/// </summary>
	[ExportMany(ContractKey = "TextBox")]
	public class TextBoxFieldFactory : IDynamicFormFieldFactory {
		/// <summary>
		/// 创建表单字段属性
		/// </summary>
		public FormFieldAttribute Create(IDictionary<string, object> fieldData) {
			var name = fieldData.GetOrDefault<string>("Name");
			var placeHolder = fieldData.GetOrDefault<string>("PlaceHolder");
			var group = fieldData.GetOrDefault<string>("Group");
			return new TextBoxFieldAttribute(name, placeHolder) { Group = group };
		}
	}
}
