using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src;

namespace ZKWeb.Plugins.Common.Admin.src {
	/// <summary>
	/// 用户管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class UserManager {
		/// <summary>
		/// 退出登录
		/// </summary>
		public virtual void Logout() {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			sessionManager.RemoveSession();
		}
	}
}
