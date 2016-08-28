using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Validators {
	/// <summary>
	/// 必填项
	/// </summary>
	[ExportMany(ContractKey = typeof(RequiredAttribute)), SingletonReuse]
	public class RequiredValidator : IFormFieldValidator {
		/// <summary>
		/// html属性键
		/// </summary>
		public const string Key = "data-val-required";

		/// <summary>
		/// 获取错误消息
		/// </summary>
		private string ErrorMessage(FormField field) {
			return string.Format(new T("{0} is required"), new T(field.Attribute.Name));
		}

		/// <summary>
		/// 添加验证使用的html属性
		/// </summary>
		public void AddHtmlAttributes(
			FormField field, object validatorAttribute, IDictionary<string, string> htmlAttributes) {
			htmlAttributes.Add(Key, ErrorMessage(field));
		}

		/// <summary>
		/// 验证值是否通过，不通过时抛出例外
		/// </summary>
		public void Validate(FormField field, object validatorAttribute, object value) {
			if (value == null || value.ToString() == "") {
				throw new BadRequestException(ErrorMessage(field));
			}
		}
	}
}
