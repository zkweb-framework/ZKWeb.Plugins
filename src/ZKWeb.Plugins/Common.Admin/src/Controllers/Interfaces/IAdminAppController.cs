using System;

namespace ZKWeb.Plugins.Common.Admin.src.Controllers.Interfaces {
	/// <summary>
	/// 后台应用控制器的接口
	/// </summary>
	public interface IAdminAppController {
		/// <summary>
		/// 应用名称
		/// </summary>
		string Name { get; }
		/// <summary>
		/// url地址
		/// </summary>
		string Url { get; }
		/// <summary>
		/// 格子的css类名
		/// </summary>
		string TileClass { get; }
		/// <summary>
		/// 图标的css类名
		/// </summary>
		string IconClass { get; }
		/// <summary>
		/// 显示此应用要求的用户类型
		/// 例如typeof(IAmAdmin)
		/// </summary>
		Type RequiredUserType { get; }
		/// <summary>
		/// 显示此应用要求的权限列表
		/// </summary>
		string[] RequiredPrivileges { get; }
	}
}
