using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Theme.VisualEditor.src.Components.VisualWidgetFormBuilders.Interfaces;
using ZKWeb.Templating.DynamicContents;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.VisualWidgetFormBuilders {
	/// <summary>
	/// 可视化编辑器中编辑模块的表单的构建器
	/// 只支持Common.Base中的表单字段类型
	/// </summary>
	[ExportMany]
	public class DefaultVisualWidgetFormBuilder : IVisualWidgetFormBuilder {
		/// <summary>
		/// 根据单个模块参数添加表单字段
		/// </summary>
		public void AddFormField(
			FormBuilder form,
			TemplateWidget widget,
			IDictionary<string, object> argument) {
			throw new NotImplementedException();
		}
	}
}
