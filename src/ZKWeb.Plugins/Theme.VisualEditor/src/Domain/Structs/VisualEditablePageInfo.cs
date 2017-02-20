using DotLiquid;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs {
	/// <summary>
	/// 可以在可视化编辑中编辑的页面信息
	/// </summary>
	public class VisualEditablePageInfo : ILiquidizable {
		/// <summary>
		/// 分组，可以是翻译前的文本
		/// </summary>
		public string Group { get; set; }
		/// <summary>
		/// 名称，可以是翻译前的文本
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 页面的Url
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public VisualEditablePageInfo() {

		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="group">分组，可以是翻译前的文本</param>
		/// <param name="name">名称，可以是翻译前的文本</param>
		/// <param name="url">页面的Url</param>
		public VisualEditablePageInfo(string group, string name, string url) {
			Group = group;
			Name = name;
			Url = url;
		}

		/// <summary>
		/// 允许描画到模板
		/// </summary>
		/// <returns></returns>
		public object ToLiquid() {
			return new { Group, Name, Url };
		}
	}
}
