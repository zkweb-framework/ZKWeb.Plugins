using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Shopping.Order.src.Managers;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.UserLoginCallbacks {
	/// <summary>
	/// 在用户登录后整合购物车商品
	/// </summary>
	[ExportMany]
	public class MergeCartProductsAfterLogin : IUserLoginCallback {
		/// <summary>
		/// 登陆前记录下的会话Id
		/// </summary>
		public string SessionId { get; set; }

		/// <summary>
		/// 查找用户，这里不负责查找
		/// </summary>
		public User FindUser(DatabaseContext context, string username) {
			return null;
		}

		/// <summary>
		/// 登陆前记录下会话Id
		/// </summary>
		public void BeforeLogin(User user) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			SessionId = sessionManager.GetSession().Id;
		}

		/// <summary>
		/// 整合登陆前添加的购物车商品
		/// </summary>
		public void AfterLogin(User user) {
			if (!string.IsNullOrEmpty(SessionId)) {
				var cartProductManager = Application.Ioc.Resolve<CartProductManager>();
				cartProductManager.MergeToUser(SessionId, user.Id);
			}
		}
	}
}
