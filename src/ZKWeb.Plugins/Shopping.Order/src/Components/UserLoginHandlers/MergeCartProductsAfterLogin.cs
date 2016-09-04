using System;
using ZKWeb.Plugins.Common.Admin.src.Components.UserLoginHandlers.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.UserLoginHandlers {
	/// <summary>
	/// 在用户登录后整合购物车商品
	/// </summary>
	[ExportMany]
	public class MergeCartProductsAfterLogin : IUserLoginHandler {
		/// <summary>
		/// 登陆前记录下的会话Id
		/// </summary>
		public Guid SessionId { get; set; }

		/// <summary>
		/// 查找用户，这里不负责查找
		/// </summary>
		public User FindUser(string username) {
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
			if (SessionId != Guid.Empty) {
				var cartProductManager = Application.Ioc.Resolve<CartProductManager>();
				cartProductManager.MergeToUser(SessionId, user.Id);
			}
		}
	}
}
