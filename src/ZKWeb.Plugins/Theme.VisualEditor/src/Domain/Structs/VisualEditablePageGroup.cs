using DotLiquid;
using System.Collections.Generic;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs {
	/// <summary>
	/// 可编辑的页面分组
	/// </summary>
	public class VisualEditablePageGroup : ILiquidizable {
		/// <summary>
		/// 分组
		/// </summary>
		public string Group { get; set; }
		/// <summary>
		/// 页面列表
		/// </summary>
		public IList<VisualEditablePageInfo> Pages { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public VisualEditablePageGroup() { }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="group">分组</param>
		/// <param name="pages">页面列表</param>
		public VisualEditablePageGroup(string group, IList<VisualEditablePageInfo> pages) {
			Group = group;
			Pages = pages;
		}

		/// <summary>
		/// 允许描画到模板
		/// </summary>
		/// <returns></returns>
		public object ToLiquid() {
			return new { Group, Pages };
		}
	}
}
