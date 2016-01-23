using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.UserPanel.src.Model;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Core;

namespace ZKWeb.Plugins.Common.UserPanel.src.UserPanelMenuProviders {
	/// <summary>
	/// 在会员中心中添加以下菜单项
	/// 会员中心
	///		首页
	/// </summary>
	[ExportMany]
	public class UserPanelIndex : IUserPanelMenuProvider {
		/// <summary>
		/// 设置显示的菜单项
		/// </summary>
		public void Setup(List<MenuItemGroup> groups) {
			var userPanelGroup = new MenuItemGroup("User Panel", "fa fa-home");
			userPanelGroup.Items.AddItemForLink(new T("Index"), "fa fa-home", "/home");
			groups.Insert(0, userPanelGroup);
		}
	}
}
