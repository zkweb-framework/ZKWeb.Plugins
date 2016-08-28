using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWeb.Web;
using ZKWebStandard.Web;
using ZKWeb.Plugins.Common.Admin.src.Components.PrivilegeProviders;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.Components.PrivilegeTranslators.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.TemplateFilters;

namespace ZKWeb.Plugins.Common.Admin.src.Domain.Services {
	/// <summary>
	/// 权限管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class PrivilegeManager : DomainServiceBase {
		/// <summary>
		/// 获取网站中注册的所有权限，并且去除重复项
		/// </summary>
		/// <returns></returns>
		public virtual List<string> GetPrivileges() {
			var providers = Application.Ioc.ResolveMany<IPrivilegesProvider>();
			var privileges = providers.SelectMany(p => p.GetPrivileges()).Distinct().ToList();
			return privileges;
		}

		/// <summary>
		/// 检查当前的用户类型是否继承了指定的类型，且是否拥有指定的权限
		/// 如果用户类型不匹配且当前请求是get则跳转到登陆页面，否则抛出403错误
		/// </summary>
		/// <param name="userType">用户类型，例如typeof(IAmAdmin)</param>
		/// <param name="privileges">要求的权限列表</param>
		public virtual void Check(Type userType, params string[] privileges) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var userTypeMatched = (user != null &&
				userType.GetTypeInfo().IsAssignableFrom(user.GetUserType().GetType()));
			var context = HttpManager.CurrentContext;
			if (context.Request.Method == HttpMethods.GET && (user == null || !userTypeMatched)) {
				// 要求管理员时跳转到后台登陆页面，否则跳转到前台登陆页面
				if (typeof(ICanUseAdminPanel).GetTypeInfo().IsAssignableFrom(userType)) {
					context.Response.RedirectByScript(BaseFilters.Url("/admin/login"));
				} else {
					context.Response.RedirectByScript(BaseFilters.Url("/user/login"));
				}
			} else if (userTypeMatched && HasPrivileges(user, privileges)) {
				// 检查通过
			} else if (privileges != null && privileges.Length > 0) {
				// 无权限403
				var translator = Application.Ioc.Resolve<IPrivilegeTranslator>();
				throw new ForbiddenException(string.Format(
					new T("Action require {0}, and {1} privileges"),
					new T(userType.Name),
					string.Join(",", privileges.Select(p => translator.Translate(p)))));
			} else {
				// 用户类型不符合，或未登录
				throw new ForbiddenException(string.Format(
					new T("Action require {0}"), new T(userType.Name)));
			}
		}

		/// <summary>
		/// 判断用户是否拥有指定的权限
		/// </summary>
		/// <param name="user">用户</param>
		/// <param name="privileges">权限列表</param>
		/// <returns></returns>
		public virtual bool HasPrivileges(User user, params string[] privileges) {
			var userType = user?.GetUserType();
			if (userType is IAmSuperAdmin) {
				// 超级管理员拥有所有权限
				return true;
			}
			if (privileges != null && privileges.Length > 0) {
				foreach (var privilege in privileges) {
					if (!user.Roles.Any(r => r.Privileges.Contains(privilege))) {
						// 未包含指定的所有权限
						return false;
					}
				}
			}
			// 检查通过
			return true;
		}
	}
}
