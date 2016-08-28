using System;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugin;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.Components.GenericConfigs;
using ZKWeb.Plugins.Common.Base.src.Controllers;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.TemplateFilters;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Admin.src.Controllers {
	/// <summary>
	/// 后台的控制器
	/// </summary>
	[ExportMany]
	public class AdminController : ControllerBase {
		/// <summary>
		/// 后台首页
		/// 显示应用列表，会根据当前用户权限进行过滤
		/// </summary>
		/// <returns></returns>
		[Action("admin")]
		public IActionResult Admin() {
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(typeof(ICanUseAdminPanel));
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var apps = Application.Ioc.ResolveMany<IAdminAppController>()
				.Where(app => app.IsAccessableFormUser(user))
				.Select(app => app.ToTileHtml()).ToList();
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
			if (user != null && user.GetUserType() is ICanUseAdminPanel) {
				return new RedirectResult(BaseFilters.Url("/admin"));
			}
			// 否则显示登陆表单
			var form = new AdminLoginForm();
			if (Request.Method == HttpMethods.POST) {
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
			return new RedirectResult(BaseFilters.Url("/admin/login"));
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
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(typeof(ICanUseAdminPanel));
			var form = new AdminAboutMeForm();
			if (Request.Method == HttpMethods.POST) {
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
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(typeof(ICanUseAdminPanel));
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var pluginManager = Application.Ioc.Resolve<PluginManager>();
			var websiteSettings = configManager.GetData<WebsiteSettings>();
			var localeSettings = configManager.GetData<LocaleSettings>();
#if NETCORE
			var serverUsername = "";
#else
			var serverUsername = Environment.UserName;
#endif
			var zkwebVersion = Application.Version;
			var zkwebFullVersion = Application.FullVersion;
			var memoryUsage = SystemUtils.GetUsedMemoryBytes() / 1024 / 1024;
			var pluginInfoTable = new StaticTableBuilder();
			pluginInfoTable.Columns.Add("DirectoryName");
			pluginInfoTable.Columns.Add("Name");
			pluginInfoTable.Columns.Add("Version");
			pluginInfoTable.Columns.Add("FullVersion");
			pluginInfoTable.Columns.Add("Description");
			pluginInfoTable.Rows.AddRange(pluginManager.Plugins.Select(p => new {
				DirectoryName = p.DirectoryName(),
				Name = new T(p.Name),
				Version = p.VersionObject(),
				FullVersion = p.Version,
				Description = new T(p.Description)
			}));
			return new TemplateResult("common.admin/about_website.html", new {
				websiteName = websiteSettings.WebsiteName,
				defaultLanguage = localeSettings.DefaultLanguage,
				defaultTimeZone = localeSettings.DefaultTimezone,
				serverUsername = serverUsername,
				zkwebVersion = zkwebVersion.ToString(),
				zkwebFullVersion = zkwebFullVersion,
				memoryUsage = memoryUsage,
				pluginInfoTable = pluginInfoTable
			});
		}
	}
}
