using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.Managers {
	/// <summary>
	/// 会话管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class SessionManager {
		/// <summary>
		/// 用于Items时，储存Session对象
		/// 用于Cookies时，储存会话Id
		/// </summary>
		public const string SessionKey = "Common.Base.Session";

		/// <summary>
		/// 获取当前Http请求对应的会话
		/// 当前没有会话时返回新的会话
		/// </summary>
		/// <returns></returns>
		public virtual Session GetSession() {
			// 从Http上下文中获取会话
			// 因为一次请求中可能会调用多次GetSession，应该确保返回同一个对象
			var context = HttpManager.CurrentContext;
			var session = context.GetData<Session>(SessionKey, null);
			if (session != null) {
				return session;
			}
			// 从数据库中获取会话
			var dabaseManager = Application.Ioc.Resolve<DatabaseManager>();
			string id = context.GetCookie(SessionKey);
			if (!string.IsNullOrEmpty(id)) {
				session = UnitOfWork.ReadData<Session, Session>(r => r.GetById(id));
			}
			// 当前没有会话时返回新的会话
			if (session == null) {
				session = new Session() {
					ReleatedId = 0,
					IpAddress = context.Request.RemoteIpAddress.ToString(),
					RememberLogin = false,
					Expires = DateTime.UtcNow.AddHours(1)
				};
				session.ReGenerateId();
			}
			context.PutData(SessionKey, session);
			return session;
		}

		/// <summary>
		/// 添加或更新当前的会话
		/// 必要时发送Cookie到浏览器
		/// </summary>
		public virtual void SaveSession() {
			var context = HttpManager.CurrentContext;
			var session = context.GetData<Session>(SessionKey, null);
			if (session == null) {
				throw new NullReferenceException("session is null");
			}
			// 添加或更新到数据库中
			var cookieSessionId = context.GetCookie(SessionKey);
			UnitOfWork.WriteData<Session>(r => {
				// 保存会话
				r.Save(ref session);
				// 检测到会话Id有变化时删除原会话
				if (cookieSessionId != session.Id) {
					r.DeleteWhere(s => s.Id == cookieSessionId);
				}
			});
			// 发送会话Cookies到客户端
			// 已存在且过期时间没有更新时不会重复发送
			if (cookieSessionId != session.Id || session.ExpiresUpdated) {
				session.ExpiresUpdated = false;
				DateTime? expires = null;
				if (session.RememberLogin) {
					expires = session.Expires.AddYears(1);
				}
				var options = new HttpCookieOptions() { Expires = expires, HttpOnly = true };
				context.PutCookie(SessionKey, session.Id, options);
			}
		}

		/// <summary>
		/// 删除当前会话
		/// </summary>
		/// <param name="removeCookie">是否同时删除Cookie</param>
		public virtual void RemoveSession(bool removeCookie) {
			// 删除Http上下文中的会话
			var context = HttpManager.CurrentContext;
			context.RemoveData(SessionKey);
			// 删除数据库中的会话
			string id = context.GetCookie(SessionKey);
			if (string.IsNullOrEmpty(id)) {
				return;
			}
			UnitOfWork.WriteData<Session>(r => r.DeleteWhere(s => s.Id == id));
			if (removeCookie) {
				// 删除客户端中的会话Cookie
				context.RemoveCookie(SessionKey);
			}
		}
	}
}
