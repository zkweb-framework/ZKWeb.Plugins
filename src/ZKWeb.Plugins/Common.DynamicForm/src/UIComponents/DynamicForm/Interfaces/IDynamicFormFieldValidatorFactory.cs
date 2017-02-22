using System;
using System.Collections.Generic;

namespace ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.Interfaces {
	/// <summary>
	/// 动态表单字段验证器的生成器
	/// 注册时需要设置键为验证器类型
	/// </summary>
	/// <example>
	/// 注册到容器的例子
	/// [ExportMany(ContractKey = "Required")]
	/// </example>
	public interface IDynamicFormFieldValidatorFactory {
		/// <summary>
		/// 创建表单字段验证器属性
		/// </summary>
		/// <param name="validatorData">验证器数据</param>
		/// <returns></returns>
		Attribute Create(IDictionary<string, object> validatorData);
	}
}
