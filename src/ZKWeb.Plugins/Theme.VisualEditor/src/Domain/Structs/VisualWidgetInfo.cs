using DotLiquid;
using ZKWeb.Templating.DynamicContents;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs {
	/// <summary>
	/// 可以在可视化编辑中使用的模块信息
	/// </summary>
	public class VisualWidgetInfo : ILiquidizable {
		/// <summary>
		/// 分组名称，可以是翻译前的文本
		/// </summary>
		public string Group { get; set; }
		/// <summary>
		/// 模板模块信息
		/// </summary>
		public TemplateWidgetInfo WidgetInfo { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public VisualWidgetInfo() {

		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="group">分组名称，可以是翻译前的文本</param>
		/// <param name="widgetInfo">模板模块信息</param>
		public VisualWidgetInfo(string group, TemplateWidgetInfo widgetInfo) {
			Group = group;
			WidgetInfo = widgetInfo;
		}

		/// <summary>
		/// 允许描画到模板
		/// </summary>
		/// <returns></returns>
		public object ToLiquid() {
			return new { Group, WidgetInfo };
		}
	}
}
