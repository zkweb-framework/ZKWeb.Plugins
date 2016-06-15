using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Web;

namespace ZKWeb.Plugins.Common.Admin.src.Model {
	/// <summary>
	/// 后台应用的接口
	/// </summary>
	public interface IAdminApp {
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
		/// 允许显示此应用的用户类型列表
		/// </summary>
		UserTypes[] AllowedUserTypes { get; }
		/// <summary>
		/// 显示此应用要求的权限列表
		/// </summary>
		string[] RequiredPrivileges { get; }
	}
}
