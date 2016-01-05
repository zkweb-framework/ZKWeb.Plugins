using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Admin.src.Extensions {
	/// <summary>
	/// 会话的扩展函数
	/// </summary>
	public static class SessionExtensions {
		/// <summary>
		/// 当前会话对应的用户
		/// </summary>
		public const string SessionUserKey = "Common.Admin.SessionUser";

		/// <summary>
		/// 获取会话对应的用户
		/// </summary>
		public static User GetUser(this Session session) {
			// 会话没有对应用户
			if (session.ReleatedId <= 0) {
				return null;
			}
			// 从HttpContext中获取，确保保存时的会话和获取时的会话是同一个
			var pair = HttpContextUtils.GetData<Tuple<Session, User>>(SessionUserKey);
			if (pair != null && pair.Item1 == session) {
				return pair.Item2;
			}
			// 从数据库中获取
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.GetContext()) {
				var user = context.Get<User>(u => u.Id == session.ReleatedId);
				if (user == null) {
					return null;
				}
				var role = user.Role; // 同时获取角色
				HttpContextUtils.PutData(SessionUserKey, Tuple.Create(session, user));
				return user;
			}
		}
	}
}
