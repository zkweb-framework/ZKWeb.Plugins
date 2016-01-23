using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Model;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src {
	/// <summary>
	/// 载入插件时的处理
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Plugin : IPlugin {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin() {
			// 注册后台应用
			Application.Ioc.ResolveMany<AdminApp>().ForEach(a => a.OnWebsiteStart());
			// 注册默认模块
			var diyManager = Application.Ioc.Resolve<DiyManager>();
			var navbarLeft = diyManager.GetArea("header_navbar_left");
			var navbarRight = diyManager.GetArea("header_navbar_right");
			navbarLeft.DefaultWidgets.Add("common.admin.widgets/user_login_info");
			navbarRight.DefaultWidgets.Add("common.admin.widgets/enter_admin_panel");
		}
	}
}
