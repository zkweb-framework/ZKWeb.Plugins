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
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Database;

namespace ZKWeb.Plugins.Common.Admin.src {
	/// <summary>
	/// 用户管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class UserManager {
		/// <summary>
		/// 记住登陆时，保留会话的天数
		/// </summary>
		public const int SessionExpireDaysWithRememebrLogin = 30;
		/// <summary>
		/// 不记住登陆时，保留会话的天数
		/// </summary>
		public const int SessionExpireDaysWithoutRememberLogin = 1;

		/// <summary>
		/// 注册用户
		/// 注册失败时会抛出例外
		/// </summary>
		public virtual void Reg(
			string username, string password, Action<User> update = null) {
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.GetContext()) {
				var user = new User();
				user.Type = UserTypes.User;
				user.Username = username;
				user.SetPassword(password);
				user.CreateTime = DateTime.UtcNow;
				context.Save(ref user, update);
				context.SaveChanges();
			}
		}

		/// <summary>
		/// 根据用户名查找用户
		/// 找不到时返回null
		/// </summary>
		public virtual User FindUser(string username) {
			var callbacks = Application.Ioc.ResolveMany<IUserLoginCallback>();
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.GetContext()) {
				// 通过回调查找用户
				foreach (var callback in callbacks) {
					var user = callback.FindUser(context, username);
					if (user != null) {
						return user;
					}
				}
				// 通过用户名查找用户
				return context.Get<User>(u => u.Username == username);
			}
		}

		/// <summary>
		/// 登陆用户
		/// 登陆失败时会抛出例外
		/// </summary>
		public virtual void Login(string username, string password, bool rememberLogin) {
			// 用户不存在或密码错误时抛出例外
			var user = FindUser(username);
			if (user == null || !user.CheckPassword(password)) {
				throw new HttpException(401, new T("Incorrect username or password"));
			}
			// 以指定用户登录
			LoginWithUser(user, rememberLogin);
		}

		/// <summary>
		/// 以指定用户登录
		/// 跳过密码等检查
		/// </summary>
		public virtual void LoginWithUser(User user, bool rememberLogin) {
			// 获取回调
			var callbacks = Application.Ioc.ResolveMany<IUserLoginCallback>().ToList();
			// 登陆前的处理
			callbacks.ForEach(c => c.BeforeLogin(user));
			// 设置会话
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			sessionManager.RemoveSession();
			var session = sessionManager.GetSession();
			session.ReGenerateId();
			session.ReleatedId = user.Id;
			session.RememberLogin = rememberLogin;
			session.SetExpiresAtLeast(TimeSpan.FromDays(session.RememberLogin ?
				SessionExpireDaysWithRememebrLogin : SessionExpireDaysWithoutRememberLogin));
			sessionManager.SaveSession();
			// 登陆后的处理
			callbacks.ForEach(c => c.AfterLogin(user));
		}

		/// <summary>
		/// 退出登录
		/// </summary>
		public virtual void Logout() {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			sessionManager.RemoveSession();
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
				referrer.AbsolutePath != "/user/logout" &&
				referrer.AbsolutePath != "/user/login") {
				return referrer.PathAndQuery;
			}
			// 默认跳转到首页
			return "/";
		}
	}
}
