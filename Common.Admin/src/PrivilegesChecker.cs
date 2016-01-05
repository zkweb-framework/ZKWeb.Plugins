using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Model;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src;

namespace ZKWeb.Plugins.Common.Admin.src {
	/// <summary>
	/// 权限检查器
	/// </summary>
	public static class PrivilegesChecker {
		/// <summary>
		/// 检查当前是否已登录，且是否拥有指定的权限
		/// 如果未登陆且当前请求是get这跳转到登陆页面，否则抛出403错误
		/// </summary>
		public static void CheckUser(params string[] privileges) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var context = HttpContext.Current;
			if (context != null && context.Request.HttpMethod == HttpMethods.GET && user == null) {
				// 跳转到登陆页面
				context.Response.Redirect("/user/login");
				return;
			} else if (HasPrivileges(user, privileges)) {
				// 检查通过
				return;
			} else if (privileges != null && privileges.Length > 0) {
				// 无权限403
				throw new HttpException(403, string.Format(
					new T("Access this page require user login and {0} privileges"),
					string.Join(", ", privileges.Select(p => new T(p)))));
			} else {
				// 未登陆403
				throw new HttpException(403, new T("Access this page require user login"));
			}
		}

		/// <summary>
		/// 检查当前登录用户是否管理员，且是否拥有指定的权限
		/// 如果用户不是管理员且当前请求是get则跳转到登陆页面，否则抛出403错误
		/// </summary>
		public static void CheckAdmin(params string[] privileges) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var context = HttpContext.Current;
			if (context != null && context.Request.HttpMethod == HttpMethods.GET &&
				(user == null || !AdminManager.AdminTypes.Contains(user.Type))) {
				// 跳转到登陆页面
				context.Response.Redirect("/admin/login");
				return;
			} else if (IsAdminAndHasPrivileges(user, privileges)) {
				// 检查通过
				return;
			} else if (privileges != null && privileges.Length > 0) {
				// 无权限403
				throw new HttpException(403, string.Format(
					new T("Access this page require admin login and {0} privileges"),
					string.Join(", ", privileges.Select(p => new T(p)))));
			} else {
				// 未登录403
				throw new HttpException(403, new T("Access this page require admin login"));
			}
		}

		/// <summary>
		/// 检查当前登录用户是否合作伙伴，且是否拥有指定的权限
		/// 如果用户不是合作伙伴且当前请求是get则跳转到登陆页面，否则抛出403错误
		/// </summary>
		public static void CheckPartner(params string[] privileges) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var context = HttpContext.Current;
			if (context != null && context.Request.HttpMethod == HttpMethods.GET &&
				(user == null || !AdminManager.ParterTypes.Contains(user.Type))) {
				// 跳转到登陆页面
				context.Response.Redirect("/admin/login");
				return;
			} else if (IsPartnerAndHasPrivileges(user, privileges)) {
				// 检查通过
				return;
			} else if (privileges != null && privileges.Length > 0) {
				// 无权限403
				throw new HttpException(403, string.Format(
					new T("Access this page require partner login and {0} privileges"),
					string.Join(", ", privileges.Select(p => new T(p)))));
			} else {
				// 未登录403
				throw new HttpException(403, new T("Access this page require partner login"));
			}
		}

		/// <summary>
		/// 检查当前登录用户是否管理员或合作伙伴，且是否拥有指定的权限
		/// 如果用户不是管理员或合作伙伴且当前请求是get则跳转到登陆页面，否则抛出403错误
		/// </summary>
		public static void CheckAdminOrPartner(params string[] privileges) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var context = HttpContext.Current;
			if (context != null && context.Request.HttpMethod == HttpMethods.GET &&
				(user == null ||
				!AdminManager.ParterTypes.Concat(AdminManager.AdminTypes).Contains(user.Type))) {
				// 跳转到登陆页面
				context.Response.Redirect("/admin/login");
				return;
			} else if (IsAdminAndHasPrivileges(user, privileges) ||
				IsPartnerAndHasPrivileges(user, privileges)) {
				// 检查通过
				return;
			} else if (privileges != null && privileges.Length > 0) {
				// 无权限403
				throw new HttpException(403, string.Format(
					new T("Access this page require admin or partner login and {0} privileges"),
					string.Join(", ", privileges.Select(p => new T(p)))));
			} else {
				// 未登录403
				throw new HttpException(403, new T("Access this page require admin or partner login"));
			}
		}

		/// <summary>
		/// 判断用户是否拥有指定的权限
		/// </summary>
		public static bool HasPrivileges(User user, params string[] privileges) {
			if (privileges != null && privileges.Length > 0) {
				var role = user.Role;
				if (role == null || !privileges.All(p => role.Privileges.Contains(p))) {
					// 无角色或未包含指定的所有权限
					return false;
				}
			}
			// 检查通过
			return true;
		}

		/// <summary>
		/// 判断当前指定用户是否管理员，且是否拥有指定的权限
		/// </summary>
		public static bool IsAdminAndHasPrivileges(User user, params string[] privileges) {
			if (!AdminManager.AdminTypes.Contains(user.Type)) {
				// 不是管理员
				return false;
			} else if (user.Type == UserTypes.SuperAdmin) {
				// 超级管理员
				return true;
			} else if (!HasPrivileges(user, privileges)) {
				// 没有指定的权限
				return false;
			}
			// 检查通过
			return true;
		}

		/// <summary>
		/// 判断当前指定用户是否合作伙伴，且是否拥有指定的权限
		/// </summary>
		/// <returns></returns>
		public static bool IsPartnerAndHasPrivileges(User user, params string[] privileges) {
			if (!AdminManager.ParterTypes.Contains(user.Type)) {
				// 不是合作伙伴
				return false;
			} else if (!HasPrivileges(user, privileges)) {
				// 没有指定的权限
				return false;
			}
			// 检查通过
			return true;
		}
	}
}
