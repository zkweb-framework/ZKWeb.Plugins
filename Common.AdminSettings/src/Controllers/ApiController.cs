using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Admin.src;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.AdminSettings.src.Model;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web;
using ZKWeb.Web.Abstractions;

namespace ZKWeb.Plugins.Common.AdminSettings.src.Controllers {
	/// <summary>
	/// Api控制器
	/// </summary>
	[ExportMany]
	public class ApiController : IController {
		/// <summary>
		/// 获取后台管理的菜单项分组列表
		/// </summary>
		/// <returns></returns>
		[Action("api/admin/settings/menu_groups")]
		public IActionResult AdminSettingsMenuGroups() {
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(UserTypesGroup.AdminOrParter);
			var groups = new List<MenuItemGroup>();
			var handlers = Application.Ioc.ResolveMany<IAdminSettingsMenuProvider>();
			handlers.ForEach(h => h.Setup(groups));
			return new JsonResult(groups);
		}
	}
}
