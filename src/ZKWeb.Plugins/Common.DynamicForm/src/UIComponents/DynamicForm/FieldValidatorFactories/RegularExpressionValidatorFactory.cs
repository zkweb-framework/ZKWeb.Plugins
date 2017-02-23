using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.FieldValidatorFactories {
	/// <summary>
	/// 生成表达式验证属性
	/// </summary>
	[ExportMany(ContractKey = "RegularExpression")]
	public class RegularExpressionValidatorFactory : IDynamicFormFieldValidatorFactory {
		/// <summary>
		/// 生成表达式验证属性
		/// </summary>
		public Attribute Create(IDictionary<string, object> validatorData) {
			var pattern = validatorData.GetOrDefault<string>("Pattern");
			return new RegularExpressionAttribute(pattern);
		}
	}
}
