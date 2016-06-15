using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Common.Admin.src.Model {
	/// <summary>
	/// 网站附加配置中使用的键
	/// </summary>
	internal class ExtraConfigKeys {
		/// <summary>
		/// 记住登陆时，保留会话的天数
		/// </summary>
		public const string SessionExpireDaysWithRememebrLogin = "Common.Admin.SessionExpireDaysWithRememebrLogin";
		/// <summary>
		/// 不记住登陆时，保留会话的天数
		/// </summary>
		public const string SessionExpireDaysWithoutRememberLogin = "Common.Admin.SessionExpireDaysWithoutRememberLogin";
		/// <summary>
		/// 头像宽度
		/// </summary>
		public const string AvatarWidth = "Common.Admin.AvatarWidth";
		/// <summary>
		/// 头像高度
		/// </summary>
		public const string AvatarHeight = "Common.Admin.AvatarHeight";
	}
}
