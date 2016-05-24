using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Admin.src.Scaffolding;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web.Interfaces;

namespace ZKWeb.Plugins.Common.AdminSettings.src.AdminApps {
	/// <summary>
	/// 后台设置
	/// 默认允许管理员或合作伙伴访问
	/// </summary>
	[ExportMany]
	public class AdminSettingsApp : AdminApp {
		public override string Name { get { return "Settings"; } }
		public override string Url { get { return "/admin/settings"; } }
		public override string TileClass { get { return "tile bg-blue"; } }
		public override string IconClass { get { return "fa fa-wrench"; } }
		public override UserTypes[] AllowedUserTypes { get { return UserTypesGroup.AdminOrParter; } }

		protected override IActionResult Action() {
			return new TemplateResult("common.admin_settings/index.html");
		}
	}
}
