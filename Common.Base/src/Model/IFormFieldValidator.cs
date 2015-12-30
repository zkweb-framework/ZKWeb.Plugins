using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 表单字段的验证器
	/// </summary>
	public interface IFormFieldValidator {
		/// <summary>
		/// 添加验证使用的html属性
		/// </summary>
		/// <param name="field">表单字段</param>
		/// <param name="validatorAttribute">验证属性</param>
		/// <param name="htmlAttributes">添加属性时保存到这里</param>
		void AddHtmlAttributes(FormField field,
			object validatorAttribute, IDictionary<string, string> htmlAttributes);

		/// <summary>
		/// 验证值是否通过，不通过时抛出例外
		/// </summary>
		/// <param name="field">表单字段</param>
		/// <param name="validatorAttribute">验证属性</param>
		/// <param name="value">需要验证的值</param>
		void Validate(FormField field, object validatorAttribute, object value);
	}
}
