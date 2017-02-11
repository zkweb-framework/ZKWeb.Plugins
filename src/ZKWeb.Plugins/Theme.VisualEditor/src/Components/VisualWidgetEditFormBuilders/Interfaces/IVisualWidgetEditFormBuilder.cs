using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Templating.DynamicContents;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.VisualWidgetEditFormBuilders.Interfaces {
	/// <summary>
	/// 可视化编辑器中编辑模块的表单的构建器接口
	/// </summary>
	public interface IVisualWidgetEditFormBuilder {
		/// <summary>
		/// 根据单个模块参数添加表单字段
		/// </summary>
		/// <param name="form">表单构建器</param>
		/// <param name="widget">模板模块对象</param>
		/// <param name="argument">单个模块参数</param>
		void AddFormField(
			FormBuilder form,
			TemplateWidget widget,
			IDictionary<string, object> argument);
	}
}
