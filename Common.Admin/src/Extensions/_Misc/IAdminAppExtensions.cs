using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Templating;

namespace ZKWeb.Plugins.Common.Admin.src.Extensions {
	/// <summary>
	/// 后台应用的接口的扩展函数
	/// </summary>
	public static class IAdminAppExtensions {
		/// <summary>
		/// 生成后台应用的格子的html
		/// </summary>
		/// <param name="app">后台应用</param>
		/// <returns></returns>
		public static HtmlString ToTileHtml(this IAdminApp app) {
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
		public static bool IsAccessableFormUser(this IAdminApp app, User user) {
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			return (app.AllowedUserTypes.Contains(user.Type) &&
				privilegeManager.HasPrivileges(user, app.RequiredPrivileges));
		}
	}
}
