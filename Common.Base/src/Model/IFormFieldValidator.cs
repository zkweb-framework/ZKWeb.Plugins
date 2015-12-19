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
		Dictionary<string, string> HtmlAttributes(object attribute);
		void Validate(object attribute, string value);
	}
}
