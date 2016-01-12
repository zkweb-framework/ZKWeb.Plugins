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
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src {
	/// <summary>
	/// 权限检查器
	/// </summary>
	public static class PrivilegesChecker {
		/// <summary>
		/// 检查当前登录用户是否指定的用户类型，且是否拥有指定的权限
		/// 如果用户类型不匹配且当前请求是get则跳转到登陆页面，否则抛出403错误
		/// </summary>
		/// <param name="type">指定的用户类型</param>
		/// <param name="privileges">要求的权限列表</param>
		public static void Check(UserTypes type, params string[] privileges) {
			Check(new[] { type }, privileges);
		}

		/// <summary>
		/// 检查当前登录用户是否指定的用户类型，且是否拥有指定的权限
		/// 如果用户类型不匹配且当前请求是get则跳转到登陆页面，否则抛出403错误
		/// </summary>
		/// <param name="types">指定的用户类型列表</param>
		/// <param name="privileges">要求的权限列表</param>
		public static void Check(UserTypes[] types, params string[] privileges) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var context = HttpContext.Current;
			if (context != null && context.Request.HttpMethod == HttpMethods.GET &&
				(user == null || !types.Contains(user.Type))) {
				// 跳转到登陆页面
				context.Response.Redirect("/admin/login");
				return;
			} else if (HasPrivileges(user, privileges)) {
				// 检查通过
				return;
			} else if (privileges != null && privileges.Length > 0) {
				// 无权限403
				throw new HttpException(403, string.Format(
					new T("Access this page require {0}, and {1} privileges"),
					string.Join(",", types.Select(t => new T(t.GetDescription()))),
					string.Join(",", privileges.Select(p => new T(p)))));
			} else {
				// 未登录403
				throw new HttpException(403, string.Format(
					new T("Access this page require {0}"),
					string.Join(",", types.Select(t => new T(t.GetDescription())))));
			}
		}

		/// <summary>
		/// 判断用户是否拥有指定的权限
		/// </summary>
		/// <param name="user">用户</param>
		/// <param name="privileges">权限列表</param>
		/// <returns></returns>
		public static bool HasPrivileges(User user, params string[] privileges) {
			if (user.Type == UserTypes.SuperAdmin) {
				// 超级管理员拥有所有权限
				return true;
			}
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
	}
}
