using System.Collections.Generic;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems {
	/// <summary>
	/// 下拉框或单选按钮使用的选项列表来源的接口
	/// </summary>
	public interface IListItemProvider {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		IEnumerable<ListItem> GetItems();
	}
}
