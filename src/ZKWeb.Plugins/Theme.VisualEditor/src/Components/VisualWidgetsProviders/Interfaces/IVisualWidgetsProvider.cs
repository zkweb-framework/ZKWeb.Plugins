using System.Collections.Generic;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.VisualWidgetsProviders.Interfaces {
	/// <summary>
	/// 获取可以在可视化编辑中使用的模块的接口
	/// </summary>
	public interface IVisualWidgetsProvider {
		/// <summary>
		/// 获取可以在可视化编辑中使用的模块列表
		/// </summary>
		/// <returns></returns>
		IEnumerable<VisualWidgetInfo> GetWidgets();
	}
}
