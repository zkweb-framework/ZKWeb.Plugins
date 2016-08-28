using System.Reflection;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Templating;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Common.Admin.src.Controllers.Extensions {
	/// <summary>
	/// 后台应用的扩展函数
	/// </summary>
	public static class IAdminAppControllerExtensions {
		/// <summary>
		/// 生成后台应用的格子的html
		/// </summary>
		/// <param name="app">后台应用</param>
		/// <returns></returns>
		public static HtmlString ToTileHtml(this IAdminAppController app) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate("common.admin/app_tile.html", new {
				name = new T(app.Name),
				tileClass = app.TileClass,
				url = app.Url,
				iconClass = app.IconClass
			});
			return new HtmlString(html);
		}

		/// <summary>
		/// 判断后台应用是否可以被指定的用户查看
		/// </summary>
		/// <param name="app">后台应用</param>
		/// <param name="user">用户</param>
		/// <returns></returns>
		public static bool IsAccessableFormUser(this IAdminAppController app, User user) {
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			var requiredUserTypeInfo = app.RequiredUserType.GetTypeInfo();
			return (requiredUserTypeInfo.IsAssignableFrom(user.GetUserType().GetType()) &&
				privilegeManager.HasPrivileges(user, app.RequiredPrivileges));
		}
	}
}
