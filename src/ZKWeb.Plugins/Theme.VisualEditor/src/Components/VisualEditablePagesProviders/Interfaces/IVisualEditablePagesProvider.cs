using System.Collections.Generic;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.VisualEditablePagesProviders.Interfaces {
	/// <summary>
	/// 获取可以在可视化编辑器中编辑的页面的接口
	/// </summary>
	public interface IVisualEditablePagesProvider {
		/// <summary>
		/// 获取可编辑的页面列表
		/// </summary>
		/// <returns></returns>
		IEnumerable<VisualEditablePageInfo> GetEditablePages();

		/// <summary>
		/// 判断页面是否可以编辑
		/// </summary>
		/// <param name="url">页面Url</param>
		/// <returns></returns>
		bool IsPageEditable(string url);
	}
}
