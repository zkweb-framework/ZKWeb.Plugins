using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems {
	/// <summary>
	/// 自定义选项分组列表提供器
	/// 请配合ListItemValueWithProvider使用
	/// </summary>
	public class CustomListItemGroupsProvider : IListItemGroupsProvider {
		/// <summary>
		/// 选项分组列表
		/// </summary>
		public IList<IGrouping<string, ListItem>> Groups { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="groups">选项分组列表</param>
		public CustomListItemGroupsProvider(IList<IGrouping<string, ListItem>> groups) {
			Groups = groups;
		}

		/// <summary>
		/// 返回选项分组列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IGrouping<string, ListItem>> GetGroups() {
			return Groups;
		}
	}
}
