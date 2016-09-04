using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.MenuItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.MenuItems.Extensions;
using ZKWeb.Plugins.Common.UserPanel.src.MenuPages.UIComponents.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.UserPanel.src.UIComponents.UserPanelMenuProviders {
	/// <summary>
	/// 在会员中心中添加以下菜单项
	/// 会员中心
	/// - 首页
	/// </summary>
	[ExportMany]
	public class UserPanelIndex : IUserPanelMenuProvider {
		/// <summary>
		/// 设置显示的菜单项
		/// </summary>
		public void Setup(IList<MenuItemGroup> groups) {
			var userPanelGroup = new MenuItemGroup("User Panel", "fa fa-home");
			userPanelGroup.Items.AddItemForLink(new T("Index"), "fa fa-home", "/home");
			groups.Insert(0, userPanelGroup);
		}
	}
}
