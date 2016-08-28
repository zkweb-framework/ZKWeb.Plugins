using System;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Ioc;
using ZKWeb.Web;
using ZKWeb.Plugins.Common.Base.src.Controllers;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;

namespace ZKWeb.Plugins.Common.Admin.src.Controllers {
	/// <summary>
	/// 用户使用的Api控制器
	/// </summary>
	[ExportMany]
	public class UserApiController : ControllerBase {
		/// <summary>
		/// 获取当前登陆的用户信息
		/// </summary>
		/// <returns></returns>
		[Action("api/user/login_info", HttpMethods.POST)]
		public IActionResult UserLoginInfo() {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var userType = user.GetUserType();
			var userManager = Application.Ioc.Resolve<UserManager>();
			return new JsonResult(new {
				userId = user?.Id,
				username = user?.Username,
				userType = user?.Type,
				userIsAdmin = userType is IAmAdmin,
				userIsParter = userType is IAmCooperationPartner,
				userCanUseAdminPanel = userType is ICanUseAdminPanel,
				avatar = userManager.GetAvatarWebPath(user?.Id ?? Guid.Empty)
			});
		}
	}
}
