using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.FieldFactories {
	/// <summary>
	/// 文件上传生成器，以url的形式自动保存
	/// </summary>
	[ExportMany(ContractKey = "FileUploaderAsUrl")]
	public class FileUploaderAsUrlFieldFactory : IDynamicFormFieldFactory {
		/// <summary>
		/// 创建表单字段属性
		/// </summary>
		public FormFieldAttribute Create(IDictionary<string, object> fieldData) {
			var name = fieldData.GetOrDefault<string>("Name");
			var extensions = fieldData.GetOrDefault<string>("Extensions");
			var maxContentsLength = fieldData.GetOrDefault<int>("MaxContentsLength");
			var uploadDirectory = fieldData.GetOrDefault<string>("UploadDirectory");
			var group = fieldData.GetOrDefault<string>("Group");
			return new FileUploaderAsUrlFieldAttribute(
				name, extensions, maxContentsLength, uploadDirectory) { Group = group };
		}
	}
}
