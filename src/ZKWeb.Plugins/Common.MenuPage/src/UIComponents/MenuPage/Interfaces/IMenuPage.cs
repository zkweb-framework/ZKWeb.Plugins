using System;

namespace ZKWeb.Plugins.Common.MenuPage.src.UIComponents.MenuPage.Interfaces {
	/// <summary>
	/// 菜单页的接口
	/// </summary>
	public interface IMenuPage : IMenuProvider {
		/// <summary>
		/// 所属的菜单分组
		/// </summary>
		string Group { get; }
		/// <summary>
		/// 菜单分组图标
		/// </summary>
		string GroupIconClass { get; }
		/// <summary>
		/// 菜单项名称
		/// </summary>
		string Name { get; }
		/// <summary>
		/// 菜单项图标
		/// </summary>
		string IconClass { get; }
		/// <summary>
		/// Url地址
		/// </summary>
		string Url { get; }
		/// <summary>
		/// 允许显示此页面的用户类型
		/// 例如typeof(IAmAdmin)
		/// </summary>
		Type RequiredUserType { get; }
		/// <summary>
		/// 显示此页面要求的权限列表
		/// </summary>
		string[] RequiredPrivileges { get; }
	}
}
