using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.Base.src.TemplateFilters;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;
using ZKWebStandard.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Admin.src.Managers {
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
				throw new ForbiddenException(new T("Incorrect username or password"));
			}
			// 当前没有任何管理员时，把这个用户设置为超级管理员
			UnitOfWork.WriteData<User>(r => {
				if (r.Count(u => UserTypesGroup.Admin.Contains(u.Type)) <= 0) {
					user.Type = UserTypes.SuperAdmin;
					r.Save(ref user);
				}
			});
			// 只允许管理员或合作伙伴登陆到后台
			if (!UserTypesGroup.AdminOrParter.Contains(user.Type)) {
				throw new ForbiddenException(new T("Sorry, You have no privileges to use admin panel."));
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
			var hasNoAdmin = UnitOfWork.ReadData<User, bool>(r => {
				return r.Count(u => UserTypesGroup.Admin.Contains(u.Type)) <= 0;
			});
			if (hasNoAdmin) {
				return new T("Website has no admin yet, the first login user will become super admin.");
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
			var request = HttpManager.CurrentContext.Request;
			var referer = request.GetReferer();
			// 来源于同一站点时，跳转到来源页面
			if (referer != null && referer.Authority == request.Host &&
				!referer.AbsolutePath.Contains("/logout") &&
				!referer.AbsolutePath.Contains("/login")) {
				return referer.PathAndQuery;
			}
			// 默认跳转到后台首页
			return BaseFilters.Url("/admin");
		}
	}
}
