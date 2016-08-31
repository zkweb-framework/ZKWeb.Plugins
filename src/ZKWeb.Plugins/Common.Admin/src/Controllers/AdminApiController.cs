using System;
using System.Linq;
using System.Net;
using ZKWeb.Cache;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Admin.src.Controllers {
	/// <summary>
	/// 管理员使用的Api控制器
	/// </summary>
	[ExportMany]
	public class AdminApiController : ControllerBase {
		/// <summary>
		/// 清理缓存
		/// 要求本地访问或管理员登陆
		/// </summary>
		/// <returns></returns>
		[Action("api/cache/clear", HttpMethods.POST)]
		public IActionResult ClearCache() {
			var request = Request;
			if (request.RemoteIpAddress != IPAddress.Loopback &&
				request.RemoteIpAddress != IPAddress.IPv6Loopback) {
				var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
				privilegeManager.Check(typeof(IAmAdmin));
			}
			var cleaners = Application.Ioc.ResolveMany<ICacheCleaner>();
			cleaners.ForEach(c => c.ClearCache());
			GC.Collect();
			return new JsonResult(new { message = new T("Clear Cache Successfully") });
		}

		/// <summary>
		/// 获取后台应用的分组列表
		/// 只返回当前用户可以查看的应用
		/// </summary>
		/// <returns></returns>
		[Action("api/admin/app_groups", HttpMethods.POST)]
		public IActionResult AppGroups() {
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(typeof(ICanUseAdminPanel));
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var appsGroups = Application.Ioc.ResolveMany<IAdminAppController>()
				.Where(app => app.IsAccessableFormUser(user))
				.GroupBy(app => app.Group)
				.Select(group => new {
					group = new T(group.Key),
					groupIconClass = group.First().GroupIconClass,
					apps = group.Select(a => new {
						name = new T(a.Name),
						url = a.Url,
						iconClass = a.IconClass,
						tileClass = a.TileClass,
						tileHtml = a.ToTileHtml()
					})
				}).ToList();
			return new JsonResult(appsGroups);
		}
	}
}
