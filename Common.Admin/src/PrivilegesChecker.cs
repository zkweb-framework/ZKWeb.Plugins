using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Database;

namespace ZKWeb.Plugins.Common.Admin.src {
	/// <summary>
	/// 权限检查器
	/// </summary>
	public static class PrivilegesChecker {
		/// <summary>
		/// 检查当前登录用户是否管理员，且是否拥有指定的权限
		/// 如果用户不是管理员且当前请求是get则跳转到登陆页面，否则抛出403错误
		/// </summary>
		public static void CheckAdmin(params string[] privileges) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 检查当前登录用户是否合作伙伴，且是否拥有指定的权限
		/// 如果用户不是合作伙伴且当前请求是get则跳转到登陆页面，否则抛出403错误
		/// </summary>
		public static void CheckPartner(params string[] privileges) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 检查当前登录用户是否管理员或合作伙伴，且是否拥有指定的权限
		/// 如果用户不是管理员或合作伙伴且当前请求是get则跳转到登陆页面，否则抛出403错误
		/// </summary>
		public static void CheckAdminOrPartner(params string[] privileges) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 判断当前指定用户是否管理员，且是否拥有指定的权限
		/// </summary>
		public static bool IsAdminAndHasPrivileges(User user, params string[] privileges) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 判断当前指定用户是否合作伙伴，且是否拥有指定的权限
		/// </summary>
		/// <returns></returns>
		public static bool IsPartnerAndHasPrivileges(User user, params string[] privileges) {
			throw new NotImplementedException();
		}
	}
}
