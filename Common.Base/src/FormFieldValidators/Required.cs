using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldValidators {
	/// <summary>
	/// 必填项
	/// </summary>
	[ExportMany(ContractKey = typeof(RequiredAttribute)), SingletonReuse]
	public class Required : IFormFieldValidator {
		/// <summary>
		/// html属性键
		/// </summary>
		public const string Key = "data-val-required";

		/// <summary>
		/// 添加验证使用的html属性
		/// </summary>
		public void AddHtmlAttributes(
			FormField field, object validatorAttribute, IDictionary<string, string> htmlAttributes) {
			var error = string.Format(new T("{0} is required"), new T(field.Attribute.Name));
			htmlAttributes.Add(Key, error);
		}

		/// <summary>
		/// 验证值是否通过，不通过时抛出例外
		/// </summary>
		public void Validate(FormField field, object validatorAttribute, object value) {
			if (value == null || value.ToString() == "") {
				var error = string.Format(new T("{0} is required"), new T(field.Attribute.Name));
				throw new HttpException(400, error);
			}
		}
	}
}
