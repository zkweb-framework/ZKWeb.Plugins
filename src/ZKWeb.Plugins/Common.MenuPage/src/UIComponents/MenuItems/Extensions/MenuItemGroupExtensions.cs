using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.MenuItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.MenuItems.Extensions;
using ZKWeb.Plugins.Common.MenuPage.src.UIComponents.MenuPages.Interfaces;

namespace ZKWeb.Plugins.Common.MenuPage.src.UIComponents.MenuItems.Extensions {
	/// <summary>
	/// 菜单页的接口的扩展函数
	/// </summary>
	public static class MenuItemGroupExtensions {
		/// <summary>
		/// 设置显示的菜单项
		/// </summary>
		/// <param name="groups">菜单项分组列表</param>
		/// <param name="page">菜单页</param>
		public static void SetupFrom(this IList<MenuItemGroup> groups, IMenuPage page) {
			// 没有权限时不显示菜单项
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			var user = sessionManager.GetSession().GetUser();
			if (user == null || !privilegeManager.HasUserType(user, page.RequiredUserType) ||
				!privilegeManager.HasPrivileges(user, page.RequiredPrivileges)) {
				return;
			}
			// 添加菜单项
			var group = groups.FirstOrDefault(g => g.Name == page.Group);
			if (group == null) {
				group = new MenuItemGroup(page.Group, page.GroupIconClass);
				groups.Add(group);
			}
			group.Items.AddItemForLink(new T(page.Name), page.IconClass, page.Url);
		}
	}
}
