using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.FieldValidatorFactories {
	/// <summary>
	/// 生成必填项属性
	/// </summary>
	[ExportMany(ContractKey = "Required")]
	public class RequiredValidatorFactory : IDynamicFormFieldValidatorFactory {
		/// <summary>
		/// 生成必填项属性
		/// </summary>
		public Attribute Create(IDictionary<string, object> validatorData) {
			return new RequiredAttribute();
		}
	}
}
