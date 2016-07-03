using System.Collections.Generic;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 表单字段处理器
	/// </summary>
	public interface IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		/// <param name="field">表单字段</param>
		/// <param name="htmlAttributes">添加到元素的html属性</param>
		/// <returns></returns>
		string Build(FormField field, IDictionary<string, string> htmlAttributes);

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		/// <param name="field">表单字段</param>
		/// <param name="values">值列表</param>
		/// <returns></returns>
		object Parse(FormField field, IList<string> values);
	}
}
