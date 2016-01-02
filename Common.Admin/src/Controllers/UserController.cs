using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Model;
using ZKWeb.Model.ActionResults;
using ZKWeb.Plugins.Common.Admin.src.Forms;

namespace ZKWeb.Plugins.Common.Admin.src.Controllers {
	/// <summary>
	/// 用户控制器
	/// </summary>
	[ExportMany]
	public class UserController : IController {
		/// <summary>
		/// 注册用户
		/// </summary>
		/// <returns></returns>
		[Action("user/reg")]
		[Action("user/reg", HttpMethods.POST)]
		public IActionResult Reg() {
			var form = new UserRegForm();
			if (HttpContext.Current.Request.HttpMethod == HttpMethods.POST) {
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
			var form = new UserLoginForm();
			if (HttpContext.Current.Request.HttpMethod == HttpMethods.POST) {
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
			return new RedirectResult("/user/login");
		}

		/// <summary>
		/// 会员中心
		/// </summary>
		/// <returns></returns>
		[Action("home")]
		public IActionResult Home() {
			return new TemplateResult("common.admin/user_home.html");
		}
	}
}
