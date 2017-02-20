using DotLiquid;
using System.Collections.Generic;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs {
	/// <summary>
	/// 可以在可视化编辑中使用的模块分组
	/// </summary>
	public class VisualWidgetGroup : ILiquidizable {
		/// <summary>
		/// 分组
		/// </summary>
		public string Group { get; set; }
		/// <summary>
		/// 模块列表
		/// </summary>
		public IList<VisualWidgetInfo> Widgets { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public VisualWidgetGroup() {
			Widgets = new List<VisualWidgetInfo>();
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="group">分组</param>
		/// <param name="widgets">模块列表</param>
		public VisualWidgetGroup(string group, IList<VisualWidgetInfo> widgets) {
			Group = group;
			Widgets = widgets;
		}

		/// <summary>
		/// 允许描画到模板
		/// </summary>
		/// <returns></returns>
		public object ToLiquid() {
			return new { Group, Widgets };
		}
	}
}
