using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldValidators {
	/// <summary>
	/// 正则表达式
	/// </summary>
	[ExportMany(ContractKey = typeof(RegularExpressionAttribute)), SingletonReuse]
	public class RegularExpression : IFormFieldValidator {
		/// <summary>
		/// 获取错误消息
		/// </summary>
		private string ErrorMessage(FormField field) {
			return string.Format(new T("{0} format is incorrect"), new T(field.Attribute.Name));
		}

		/// <summary>
		/// 添加验证使用的html属性
		/// </summary>
		public void AddHtmlAttributes(
			FormField field, object validatorAttribute, IDictionary<string, string> htmlAttributes) {
			var attribtue = (RegularExpressionAttribute)validatorAttribute;
			htmlAttributes.Add("data-val-regex-pattern", attribtue.Pattern);
			htmlAttributes.Add("data-val-regex", ErrorMessage(field));
		}

		/// <summary>
		/// 验证值是否通过，不通过时抛出例外
		/// </summary>
		public void Validate(FormField field, object validatorAttribute, object value) {
			var str = (value ?? "").ToString();
			if (string.IsNullOrEmpty(str)) {
				return; // 空白文本应该由Required处理
			}
			var attribtue = (RegularExpressionAttribute)validatorAttribute;
			if (!Regex.IsMatch(str, attribtue.Pattern)) {
				throw new HttpException(400, ErrorMessage(field));
			}
		}
	}
}
