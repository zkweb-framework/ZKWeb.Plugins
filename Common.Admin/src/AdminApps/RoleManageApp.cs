using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Model;

namespace ZKWeb.Plugins.Common.Admin.src.AdminApps {
	/// <summary>
	/// 角色管理
	/// 要求超级管理员
	/// </summary>
	[ExportMany]
	public class RoleManageApp : AdminAppBuilder<UserRole> {
		/// <summary>
		/// 应用信息
		/// </summary>
		public override string Name { get { return "Role Manage"; } }
		public override string Url { get { return "/admin/roles"; } }
		public override string TileClass { get { return "tile bg-blue-hoki"; } }
		public override string IconClass { get { return "fa fa-legal"; } }
		public override UserTypes[] AllowedUserTypes { get { return new[] { UserTypes.SuperAdmin }; } }
	}
}
