using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;

namespace ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.Interfaces {
	/// <summary>
	/// 修改构建后的动态表单
	/// </summary>
	public interface IDynamicFormModifier {
		/// <summary>
		/// 修改表单
		/// </summary>
		/// <param name="form">表单</param>
		/// <param name="fieldDatas">字段数据列表</param>
		void Modify(FormBuilder form, IList<IDictionary<string, object>> fieldDatas);
	}
}
