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
	/// 管理员管理
	/// 要求超级管理员
	/// </summary>
	[ExportMany]
	public class AdminManageApp : AdminAppBuilder<User> {
		/// <summary>
		/// 应用信息
		/// </summary>
		public override string Name { get { return "Admin Manage"; } }
		public override string Url { get { return "/admin/admins"; } }
		public override string TileClass { get { return "tile bg-blue-hoki"; } }
		public override string IconClass { get { return "fa fa-user-secret"; } }
		public override UserTypes[] AllowedUserTypes { get { return new[] { UserTypes.SuperAdmin }; } }
	}
}
