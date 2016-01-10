using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Admin.src.AdminApps {
	/// <summary>
	/// 用户管理
	/// </summary>
	[ExportMany]
	public class UserManageApp : AdminAppBuilder<User, UserManageApp> {
		public override string Name { get { return "User Manage"; } }
		public override string Url { get { return "/admin/users"; } }
		public override string TileClass { get { return "tile bg-blue-hoki"; } }
		public override string IconClass { get { return "fa fa-user"; } }
		protected override IAjaxTableSearchHandler<User> GetSearchHandler() { throw new NotImplementedException(); }
		protected override FormBuilder GetAddForm() { return new FormBuilder(); }
		protected override FormBuilder GetEditForm() { return new FormBuilder(); }
	}
}
