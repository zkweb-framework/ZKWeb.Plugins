using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.UserPanel.src.Model;

namespace ZKWeb.Plugins.Common.UserContact.src.UserPanelMenuProviders {
	/// <summary>
	/// 在用户中心添加联系信息菜单项
	/// </summary>
	[ExportMany]
	public class ContactInformation : IUserPanelMenuProvider {
		/// <summary>
		/// 设置显示的菜单项
		/// </summary>
		public void Setup(List<MenuItemGroup> groups) {
			var accountManage = groups.FirstOrDefault(g => g.Name == "Account Manage");
			if (accountManage != null) {
				accountManage.Items.AddItemForLink(
				new T("Contact Information"), "fa fa-phone", "/home/contact_info");
			}
		}
	}
}
