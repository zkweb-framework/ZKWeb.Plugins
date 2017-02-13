using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Theme.VisualEditor.src.Components.VisualWidgetsProviders.Interfaces;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.VisualWidgetsProviders {
	/// <summary>
	/// 获取可以在可视化编辑中使用的模块
	/// 获取流程:
	/// - 遍历所有插件
	/// - 遍历插件下的templates文件夹
	/// - 查找后缀名是.widget的文件
	/// </summary>
	[ExportMany, SingletonReuse]
	public class DefaultVisualWidgetsProvider : IVisualWidgetsProvider {
		/// <summary>
		/// 获取可以在可视化编辑中使用的模块列表
		/// </summary>
		public IEnumerable<VisualWidgetInfo> GetWidgets() {
			throw new NotImplementedException();
		}
	}
}
