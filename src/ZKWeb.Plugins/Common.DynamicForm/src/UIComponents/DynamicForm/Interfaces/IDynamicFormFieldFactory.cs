using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;

namespace ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.Interfaces {
	/// <summary>
	/// 动态表单字段的生成器
	/// 注册时需要设置键为字段类型
	/// </summary>
	/// <example>
	/// 注册到容器的例子
	/// [ExportMany(ContractKey = "TextBox")]
	/// </example>
	public interface IDynamicFormFieldFactory {
		/// <summary>
		/// 创建表单字段属性
		/// </summary>
		/// <param name="fieldData">字段数据</param>
		FormFieldAttribute Create(IDictionary<string, object> fieldData);
	}
}
