using ZKWeb.Templating.DynamicContents;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs {
	/// <summary>
	/// 可以在可视化编辑中使用的模块信息
	/// </summary>
	public class VisualWidgetInfo {
		/// <summary>
		/// 模板模块信息
		/// </summary>
		public TemplateWidgetInfo WidgetInfo { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public VisualWidgetInfo() { }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="widgetInfo">模板模块信息</param>
		public VisualWidgetInfo(TemplateWidgetInfo widgetInfo) {
			WidgetInfo = widgetInfo;
		}
	}
}
