using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 勾选框使用的选项分组列表来源的接口
	/// </summary>
	public interface IListItemGroupsProvider {
		/// <summary>
		/// 获取分组列表
		/// </summary>
		/// <returns></returns>
		IEnumerable<IGrouping<string, ListItem>> GetGroups();
	}
}
