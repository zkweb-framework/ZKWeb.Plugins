using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.FieldValidatorFactories {
	/// <summary>
	/// 生成长度验证属性
	/// </summary>
	[ExportMany(ContractKey = "StringLength")]
	public class StringLengthValidatorFactory : IDynamicFormFieldValidatorFactory {
		/// <summary>
		/// 生成长度验证属性
		/// </summary>
		public Attribute Create(IDictionary<string, object> validatorData) {
			var maximumLength = validatorData.GetOrDefault<int>("MaximumLength");
			var minimumLength = validatorData.GetOrDefault<int>("MinimumLength");
			return new StringLengthAttribute(maximumLength) { MinimumLength = minimumLength };
		}
	}
}
