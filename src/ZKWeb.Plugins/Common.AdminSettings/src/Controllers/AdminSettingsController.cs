using System;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.AdminSettings.src.Controllers {
	/// <summary>
	/// 后台设置
	/// 默认允许管理员或合作伙伴访问
	/// </summary>
	[ExportMany]
	public class AdminSettingsController : SimpleAdminAppControllerBase {
		public override string Group { get { return "System"; } }
		public override string GroupIconClass { get { return "fa fa-gear"; } }
		public override string Name { get { return "Settings"; } }
		public override string Url { get { return "/admin/settings"; } }
		public override string TileClass { get { return "tile bg-blue"; } }
		public override string IconClass { get { return "fa fa-wrench"; } }
		public override Type RequiredUserType { get { return typeof(ICanUseAdminPanel); } }

		protected override IActionResult Action() {
			return new TemplateResult("common.admin_settings/index.html");
		}
	}
}
