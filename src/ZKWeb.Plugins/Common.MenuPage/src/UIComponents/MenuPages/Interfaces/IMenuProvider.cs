using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.MenuItems;

namespace ZKWeb.Plugins.Common.MenuPage.src.UIComponents.MenuPages.Interfaces {
	/// <summary>
	/// 菜单分组和菜单项的提供器接口
	/// 这个接口需要再次继承，请勿直接使用
	/// </summary>
	public interface IMenuProvider {
		/// <summary>
		/// 设置显示的菜单项
		/// </summary>
		/// <param name="groups">菜单项分组列表</param>
		void Setup(IList<MenuItemGroup> groups);
	}
}
