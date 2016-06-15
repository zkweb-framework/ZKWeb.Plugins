using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Admin.src.ListItemProviders {
	/// <summary>
	/// 权限的选项分组列表提供器
	/// </summary>
	public class PrivilegesListItemGroupsProvider : IListItemGroupsProvider {
		/// <summary>
		/// 获取分组列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IGrouping<string, ListItem>> GetGroups() {
			// 获取所有权限
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			var privileges = privilegeManager.GetPrivileges();
			// 如果权限中包含':'，则以前半部分为分组名称，否则分组名称为Other
			return privileges.Select(p => {
				var index = p.IndexOf(':');
				var group = index > 0 ? p.Substring(0, index) : "Other";
				var name = index > 0 ? p.Substring(index + 1) : p;
				return new { name, group, value = p };
			}).GroupBy(p => p.group, p => new ListItem(p.name, p.value));
		}
	}
}
