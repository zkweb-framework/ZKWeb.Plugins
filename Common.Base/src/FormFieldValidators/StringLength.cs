using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldValidators {
	/// <summary>
	/// 字符串长度
	/// </summary>
	[ExportMany(ContractKey = typeof(StringLengthAttribute)), SingletonReuse]
	public class StringLength : IFormFieldValidator {
		/// <summary>
		/// 获取错误消息
		/// </summary>
		private string ErrorMessage(FormField field, StringLengthAttribute attribute) {
			if (attribute.MaximumLength == attribute.MinimumLength) {
				return string.Format(new T("Length of {0} must be {1}"),
					new T(field.Attribute.Name), attribute.MinimumLength);
			}
			return string.Format(new T("Length of {0} must between {1} and {2}"),
				new T(field.Attribute.Name), attribute.MinimumLength, attribute.MaximumLength);
		}

		/// <summary>
		/// 添加验证使用的html属性
		/// </summary>
		public void AddHtmlAttributes(
			FormField field, object validatorAttribute, IDictionary<string, string> htmlAttributes) {
			var attribute = (StringLengthAttribute)validatorAttribute;
			htmlAttributes["data-val-length"] = ErrorMessage(field, attribute);
			htmlAttributes["data-val-length-max"] = attribute.MaximumLength.ToString();
			htmlAttributes["data-val-length-min"] = attribute.MinimumLength.ToString();
		}

		/// <summary>
		/// 验证值是否通过，不通过时抛出例外
		/// </summary>
		public void Validate(FormField field, object validatorAttribute, object value) {
			var str = (value ?? "").ToString();
			if (string.IsNullOrEmpty(str)) {
				return; // 空白文本应该由Required处理
			}
			var attribute = (StringLengthAttribute)validatorAttribute;
			if (str.Length < attribute.MinimumLength || str.Length > attribute.MaximumLength) {
				throw new HttpException(400, ErrorMessage(field, attribute));
			}
		}
	}
}
