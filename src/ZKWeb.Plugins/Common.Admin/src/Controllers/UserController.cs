using System;
using ZKWebStandard.Ioc;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web;
using ZKWeb.Plugins.Common.Base.src.Controllers;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.TemplateFilters;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Templating.DynamicContents;

namespace ZKWeb.Plugins.Common.Admin.src.Controllers {
	/// <summary>
	/// 用户控制器
	/// </summary>
	[ExportMany]
	public class UserController : ControllerBase {
		/// <summary>
		/// 注册用户
		/// </summary>
		/// <returns></returns>
		[Action("user/reg")]
		[Action("user/reg", HttpMethods.POST)]
		public IActionResult Reg() {
			// 已登录时跳转到用户中心
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			if (user != null) {
				return new RedirectResult(BaseFilters.Url("/home"));
			}
			// 否则显示注册表单
			var form = new UserRegForm();
			if (Request.Method == HttpMethods.POST) {
				return new JsonResult(form.Submit());
			} else {
				form.Bind();
				return new TemplateResult("common.admin/user_reg.html", new { form });
			}
		}

		/// <summary>
		/// 登陆用户
		/// </summary>
		/// <returns></returns>
		[Action("user/login")]
		[Action("user/login", HttpMethods.POST)]
		public IActionResult Login() {
			// 已登录时跳转到用户中心
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			if (user != null) {
				return new RedirectResult(BaseFilters.Url("/home"));
			}
			// 否则显示登陆表单
			var form = new UserLoginForm();
			if (Request.Method == HttpMethods.POST) {
				return new JsonResult(form.Submit());
			} else {
				form.Bind();
				return new TemplateResult("common.admin/user_login.html", new { form });
			}
		}

		/// <summary>
		/// 退出登录
		/// </summary>
		/// <returns></returns>
		[Action("user/logout", HttpMethods.POST)]
		public IActionResult Logout() {
			var userManager = Application.Ioc.Resolve<UserManager>();
			userManager.Logout();
			return new RedirectResult(BaseFilters.Url("/user/login"));
		}

		/// <summary>
		/// 用户中心
		/// </summary>
		/// <returns></returns>
		[Action("home")]
		public IActionResult Home() {
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(typeof(IUserType)); // 只要求登陆
			return new TemplateResult("common.admin/user_home.html");
		}
	}
}
