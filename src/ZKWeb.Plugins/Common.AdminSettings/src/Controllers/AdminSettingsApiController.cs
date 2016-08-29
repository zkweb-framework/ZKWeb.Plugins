using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.AdminSettings.src.UIComponents.MenuPages.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.MenuItems;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.AdminSettings.src.Controllers {
	/// <summary>
	/// 后台设置的Api控制器
	/// </summary>
	public class AdminSettingsApiController : ControllerBase {
		/// <summary>
		/// 获取后台设置的菜单项分组列表
		/// </summary>
		/// <returns></returns>
		[Action("api/admin/settings/menu_groups")]
		public IActionResult AdminSettingsMenuGroups() {
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(typeof(ICanUseAdminPanel));
			var groups = new List<MenuItemGroup>();
			var handlers = Application.Ioc.ResolveMany<IAdminSettingsMenuProvider>();
			handlers.ForEach(h => h.Setup(groups));
			return new JsonResult(groups);
		}
	}
}
