using DryIoc;
using DryIocAttributes;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Base.src {
	/// <summary>
	/// 会话管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class SessionManager {
		/// <summary>
		/// 用于HttpContext.Items时，储存Session对象
		/// 用于Cookies时，储存会话Id
		/// </summary>
		public const string SessionKey = "Common.Base.Session";

		/// <summary>
		/// 获取当前Http请求对应的会话
		/// 当前没有会话时返回新的会话
		/// </summary>
		/// <returns></returns>
		public Session GetSession() {
			// 从HttpContext中获取会话
			// 因为一次请求中可能会调用多次GetSession，应该确保返回同一个对象
			var session = HttpContextUtils.GetData<Session>(SessionKey, null);
			if (session != null) {
				return session;
			}
			// 从数据库中获取会话
			var dabaseManager = Application.Ioc.Resolve<DatabaseManager>();
			string id = HttpContextUtils.GetCookie(SessionKey);
			if (!string.IsNullOrEmpty(id)) {
				using (var context = dabaseManager.GetContext()) {
					session = context.Get<Session>(s => s.Id == id);
				}
			}
			// 当前没有会话时返回新的会话
			if (session == null) {
				session = new Session()
				{
					ReleatedId = 0,
					IpAddress = HttpContextUtils.GetClientIpAddress(),
					RememberLogin = false,
					Expires = DateTime.UtcNow.AddHours(1)
				};
				session.ReGenerateId();
			}
			HttpContextUtils.PutData(SessionKey, session);
			return session;
		}

		/// <summary>
		/// 添加或更新当前的会话
		/// 必要时发送Cookie到浏览器
		/// </summary>
		public void SaveSession() {
			var session = HttpContextUtils.GetData<Session>(SessionKey, null);
			if (session == null) {
				throw new NullReferenceException("session is null");
			}
			// 添加或更新到数据库中
			var cookieSessionId = HttpContextUtils.GetCookie(SessionKey);
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.GetContext()) {
				// 保存会话
				context.Save(ref session);
				// 检测到会话Id有变化时删除原会话
				if (cookieSessionId != session.Id) {
					context.DeleteWhere<Session>(s => s.Id == cookieSessionId);
				}
				// 保存修改
				context.SaveChanges();
			}
			// 发送会话Cookies到客户端
			// 已存在且过期时间没有更新时不会重复发送
			if (cookieSessionId != session.Id || session.ExpiresUpdated) {
				session.ExpiresUpdated = false;
				DateTime? expires = null;
				if (session.RememberLogin) {
					expires = session.Expires.AddYears(1);
				}
				HttpContextUtils.PutCookie(SessionKey, session.Id, expires, true);
			}
		}

		/// <summary>
		/// 删除当前会话
		/// 同时删除浏览器中的Cookie
		/// </summary>
		public void RemoveSession() {
			// 删除HttpContext中的会话
			HttpContextUtils.RemoveData(SessionKey);
			// 删除数据库中的会话
			string id = HttpContextUtils.GetCookie(SessionKey);
			if (string.IsNullOrEmpty(id)) {
				return;
			}
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.GetContext()) {
				context.DeleteWhere<Session>(s => s.Id == id);
				context.SaveChanges();
			}
			// 删除客户端中的会话Cookies
			HttpContextUtils.RemoveCookie(SessionKey);
		}
	}
}
