using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 表单字段处理器
	/// </summary>
	public interface IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		/// <param name="field">表单字段</param>
		/// <param name="ValidatorAttributes">验证属性</param>
		/// <returns></returns>
		string Build(FormBuilder.FormField field, Dictionary<string, string> ValidatorAttributes);

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		/// <param name="field">表单字段</param>
		/// <returns></returns>
		object Parse(FormBuilder.FormField field, string value);
	}
}
