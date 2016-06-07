using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Web.ActionResults;
using ZKWeb.Plugins.Common.Admin.src;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.UserPanel.src.Model;
using ZKWeb.Utils.Extensions;
using ZKWeb.Web.Interfaces;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.UserPanel.src.Controllers {
	/// <summary>
	/// Api控制器
	/// </summary>
	[ExportMany]
	public class ApiController : IController {
		/// <summary>
		/// 获取用户中心的菜单项分组列表
		/// </summary>
		/// <returns></returns>
		[Action("api/user/panel/menu_groups")]
		public IActionResult UserPanelMenuGroups() {
			PrivilegesChecker.Check(UserTypesGroup.All);
			var groups = new List<MenuItemGroup>();
			var providers = Application.Ioc.ResolveMany<IUserPanelMenuProvider>();
			providers.ForEach(h => h.Setup(groups));
			return new JsonResult(groups);
		}
	}
}
