using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Admin.src.Database;

namespace ZKWeb.Plugins.Common.Admin.src.Model {
	/// <summary>
	/// 用户登录回调
	/// </summary>
	public interface IUserLoginCallback {
		/// <summary>
		/// 根据用户名查找用户
		/// 不支持此函数或没有查找到时返回null
		/// </summary>
		/// <param name="context">数据库上下文</param>
		/// <param name="username">用户名</param>
		/// <returns></returns>
		User FindUser(DatabaseContext context, string username);

		/// <summary>
		/// 登陆前的处理
		/// </summary>
		/// <param name="user">用户</param>
		void BeforeLogin(User user);

		/// <summary>
		/// 登陆后的处理
		/// </summary>
		/// <param name="user">用户</param>
		void AfterLogin(User user);
	}
}
