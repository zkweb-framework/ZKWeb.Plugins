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
	/// 用户管理
	/// </summary>
	[ExportMany]
	public class UserManageApp : AdminAppBuilder<User> {
		/// <summary>
		/// 应用信息
		/// </summary>
		public override string Name { get { return "User Manage"; } }
		public override string Url { get { return "/admin/users"; } }
		public override string TileClass { get { return "tile bg-blue-hoki"; } }
		public override string IconClass { get { return "fa fa-user"; } }
	}
}
