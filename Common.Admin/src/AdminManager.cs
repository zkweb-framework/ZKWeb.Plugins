using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src;

namespace ZKWeb.Plugins.Common.Admin.src {
	/// <summary>
	/// 管理员管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class AdminManager {
		/// <summary>
		/// 登陆管理员
		/// 登陆失败时会抛出例外
		/// </summary>
		public virtual void Login(string username, string password, bool rememberLogin) {
			var userManager = Application.Ioc.Resolve<UserManager>();
			var user = userManager.FindUser(username);
			// 用户不存在或密码错误时抛出例外
			if (user == null || !user.CheckPassword(password)) {
				throw new HttpException(401, new T("Incorrect username or password"));
			}
			// 当前没有任何管理员时，把这个用户设置为超级管理员
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.GetContext()) {
				if (context.Count<User>(u => UserTypesGroup.Admin.Contains(u.Type)) <= 0) {
					user.Type = UserTypes.SuperAdmin;
					context.Save(ref user);
					context.SaveChanges();
				}
			}
			// 只允许管理员或合作伙伴登陆到后台
			if (!UserTypesGroup.AdminOrParter.Contains(user.Type)) {
				throw new HttpException(401, new T("Sorry, You have no privileges to use admin panel."));
			}
			// 以指定用户登录
			userManager.LoginWithUser(user, rememberLogin);
		}

		/// <summary>
		/// 获取登录管理员时的警告信息
		/// </summary>
		/// <returns></returns>
		public virtual string GetLoginWarning() {
			// 当前没有任何管理员，第一次登录的用户将成为超级管理员
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.GetContext()) {
				if (context.Count<User>(u => UserTypesGroup.Admin.Contains(u.Type)) <= 0) {
					return new T("Website has no admin yet, the first login user will become super admin.");
				}
			}
			// 警告当前登录的用户非管理员
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			if (user != null) {
				return new T("You have already logged in, continue will replace the logged in user.");
			}
			return null;
		}

		/// <summary>
		/// 获取登录后应该跳转到的url
		/// </summary>
		/// <returns></returns>
		public virtual string GetUrlRedirectAfterLogin() {
			var request = HttpContext.Current.Request;
			var referrer = request.UrlReferrer;
			// 来源于同一站点时，跳转到来源页面
			if (referrer != null && referrer.Host == request.Url.Host &&
				referrer.AbsolutePath != "/admin/logout") {
				return referrer.PathAndQuery;
			}
			// 默认跳转到后台首页
			return "/admin";
		}
	}
}
