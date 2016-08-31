using System;

namespace ZKWeb.Plugins.Common.Admin.src.Controllers.Interfaces {
	/// <summary>
	/// 后台应用控制器的接口
	/// </summary>
	public interface IAdminAppController {
		/// <summary>
		/// 分组名称
		/// </summary>
		string Group { get; }
		/// <summary>
		/// 分组图标的css类
		/// 分组下有多个应用时，只使用第一个图标
		/// </summary>
		string GroupIconClass { get; }
		/// <summary>
		/// 应用名称
		/// </summary>
		string Name { get; }
		/// <summary>
		/// url地址
		/// </summary>
		string Url { get; }
		/// <summary>
		/// 格子的css类
		/// </summary>
		string TileClass { get; }
		/// <summary>
		/// 图标的css类
		/// </summary>
		string IconClass { get; }
		/// <summary>
		/// 显示此应用要求的用户类型
		/// 例如typeof(IAmAdmin)
		/// </summary>
		Type RequiredUserType { get; }
		/// <summary>
		/// 显示此应用要求的权限列表
		/// 一般是查看权限
		/// </summary>
		string[] RequiredPrivileges { get; }
	}
}
