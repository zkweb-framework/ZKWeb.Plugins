using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWebStandard.Utils;

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
			// 从http上下文中获取，确保保存时的会话和获取时的会话是同一个
			var pair = HttpContextUtils.GetData<Tuple<Session, User>>(SessionUserKey);
			if (pair != null && pair.Item1 == session) {
				return pair.Item2;
			}
			// 从数据库中获取
			var user = UnitOfWork.ReadData<User, User>(r => {
				var u = r.GetById(session.ReleatedId);
				if (u != null) {
					var _ = u.Roles.SelectMany(role => role.Privileges).ToList(); // 预读数据
				}
				return u;
			});
			if (user != null) {
				HttpContextUtils.PutData(SessionUserKey, Tuple.Create(session, user));
			}
			return user;
		}
	}
}
