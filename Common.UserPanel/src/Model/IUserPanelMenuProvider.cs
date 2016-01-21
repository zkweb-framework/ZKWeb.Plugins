using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.UserPanel.src.Model {
	/// <summary>
	/// 用户中心菜单项的提供器接口
	/// </summary>
	public interface IUserPanelMenuProvider {
		/// <summary>
		/// 设置显示的菜单项
		/// </summary>
		/// <param name="groups">菜单项分组列表</param>
		void Setup(List<MenuItemGroup> groups);
	}
}
