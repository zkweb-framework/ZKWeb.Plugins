using Common.Minimal.Model.Extensions;
using DotLiquid;
using DryIoc;
using DryIocAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using ZKWeb;
using ZKWeb.Web.ActionResults;
using ZKWeb.Plugins.Common.Admin.src;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Forms;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Config;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Utils.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Web.Interfaces;
using ZKWeb.Plugin;
using ZKWeb.Localize;

namespace ZKWeb.Plugins.Common.Base.src.Controllers {
	/// <summary>
	/// 后台的控制器
	/// </summary>
	[ExportMany]
	public class AdminController : IController {
		/// <summary>
		/// 后台首页
		/// 显示应用列表，会根据当前用户权限进行过滤
		/// </summary>
		/// <returns></returns>
		[Action("admin")]
		public IActionResult Admin() {
			PrivilegesChecker.Check(UserTypesGroup.AdminOrParter);
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var apps = Application.Ioc.ResolveMany<AdminApp>();
			apps = apps.Where(app =>
				app.AllowedUserTypes.Contains(user.Type) &&
				PrivilegesChecker.HasPrivileges(user, app.RequiredPrivileges));
			return new TemplateResult("common.admin/admin_index.html", new { apps });
		}

		/// <summary>
		/// 后台登陆页
		/// </summary>
		/// <returns></returns>
		[Action("admin/login")]
		[Action("admin/login", HttpMethods.POST)]
		public IActionResult Login() {
			// 已登录时跳转到后台首页
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			if (user != null && UserTypesGroup.AdminOrParter.Contains(user.Type)) {
				return new RedirectResult("/admin");
			}
			// 否则显示登陆表单
			var form = new AdminLoginForm();
			if (HttpContext.Current.Request.HttpMethod == HttpMethods.POST) {
				return new JsonResult(form.Submit());
			} else {
				form.Bind();
				var adminManager = Application.Ioc.Resolve<AdminManager>();
				var warning = adminManager.GetLoginWarning();
				return new TemplateResult("common.admin/admin_login.html", new { form, warning });
			}
		}

		/// <summary>
		/// 退出后台登陆
		/// </summary>
		/// <returns></returns>
		[Action("admin/logout", HttpMethods.POST)]
		public IActionResult Logout() {
			var userManager = Application.Ioc.Resolve<UserManager>();
			userManager.Logout();
			return new RedirectResult("/admin/login");
		}

		/// <summary>
		/// 工作区
		/// </summary>
		/// <returns></returns>
		[Action("admin/workspace")]
		public IActionResult Workspace() {
			return new TemplateResult("common.admin/workspace.html");
		}

		/// <summary>
		/// 关于我
		/// </summary>
		/// <returns></returns>
		[Action("admin/about_me")]
		[Action("admin/about_me", HttpMethods.POST)]
		public IActionResult AboutMe() {
			PrivilegesChecker.Check(UserTypesGroup.AdminOrParter);
			var form = new AdminAboutMeForm();
			if (HttpContext.Current.Request.HttpMethod == HttpMethods.POST) {
				return new JsonResult(form.Submit());
			} else {
				form.Bind();
				return new TemplateResult("common.admin/about_me.html", new { form });
			}
		}

		/// <summary>
		/// 关于网站
		/// </summary>
		/// <returns></returns>
		[Action("admin/about_website")]
		public IActionResult AboutWebsite() {
			PrivilegesChecker.Check(UserTypesGroup.AdminOrParter);
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var pluginManager = Application.Ioc.Resolve<PluginManager>();
			var websiteSettings = configManager.GetData<WebsiteSettings>();
			var localeSettings = configManager.GetData<LocaleSettings>();
			var serverUsername = Environment.UserName;
			var serverVariables = HttpContext.Current.Request.ServerVariables;
			var hostingInfoTable = new DataTable();
			hostingInfoTable.Columns.Add("Name");
			hostingInfoTable.Columns.Add("Value");
			serverVariables.AllKeys.ForEach(k =>
				hostingInfoTable.Rows.Add(k, serverVariables[k]));
			var pluginInfoTable = new DataTable();
			pluginInfoTable.Columns.Add("DirectoryName");
			pluginInfoTable.Columns.Add("Name");
			pluginInfoTable.Columns.Add("Description");
			pluginManager.Plugins.ForEach(p =>
				pluginInfoTable.Rows.Add(p.DirectoryName(), new T(p.Name), new T(p.Description)));
			return new TemplateResult("common.admin/about_website.html", new {
				websiteName = websiteSettings.WebsiteName,
				defaultLanguage = localeSettings.DefaultLanguage,
				defaultTimeZone = localeSettings.DefaultTimezone,
				serverUsername = serverUsername,
				hostingInfoTable = hostingInfoTable.ToHtml(),
				pluginInfoTable = pluginInfoTable.ToHtml()
			});
		}
	}
}
