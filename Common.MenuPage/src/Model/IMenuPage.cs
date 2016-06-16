using ZKWeb.Plugins.Common.Admin.src.Model;

namespace ZKWeb.Plugins.Common.MenuPage.src.Model {
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
		/// 允许显示此页面的用户类型列表
		/// </summary>
		UserTypes[] AllowedUserTypes { get; }
		/// <summary>
		/// 显示此页面要求的权限列表
		/// </summary>
		string[] RequiredPrivileges { get; }
	}
}
