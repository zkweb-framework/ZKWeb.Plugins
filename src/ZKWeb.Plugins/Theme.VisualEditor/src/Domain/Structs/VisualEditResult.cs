using System.Collections.Generic;
using ZKWeb.Templating.DynamicContents;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs {
	/// <summary>
	/// 可视化编辑结果
	/// </summary>
	public class VisualEditResult {
		/// <summary>
		/// 区域的编辑结果列表
		/// </summary>
		public IList<AreaEditResult> Areas { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public VisualEditResult() {
			Areas = new List<AreaEditResult>();
		}

		/// <summary>
		/// 单个区域的编辑结果
		/// </summary>
		public class AreaEditResult {
			/// <summary>
			/// 区域Id
			/// </summary>
			public string AreaId { get; set; }
			/// <summary>
			/// 模块列表
			/// </summary>
			public IList<TemplateWidget> Widgets { get; set; }

			/// <summary>
			/// 初始化
			/// </summary>
			public AreaEditResult() {
				AreaId = null;
				Widgets = new List<TemplateWidget>();
			}
		}
	}
}
