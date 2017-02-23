using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.FieldFactories {
	/// <summary>
	/// 文件上传生成器
	/// </summary>
	[ExportMany(ContractKey = "FileUploader")]
	public class FileUploaderFieldFactory : IDynamicFormFieldFactory {
		/// <summary>
		/// 创建表单字段属性
		/// </summary>
		public FormFieldAttribute Create(IDictionary<string, object> fieldData) {
			var name = fieldData.GetOrDefault<string>("Name");
			var extensions = fieldData.GetOrDefault<string>("Extensions");
			var maxContentsLength = fieldData.GetOrDefault<int>("MaxContentsLength");
			var group = fieldData.GetOrDefault<string>("Group");
			return new FileUploaderFieldAttribute(name, extensions, maxContentsLength) { Group = group };
		}
	}
}
