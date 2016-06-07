using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugin.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Templating.AreaSupport;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.IocContainer;

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
			// 注册默认模块
			var areaManager = Application.Ioc.Resolve<TemplateAreaManager>();
			areaManager.GetArea("header_navbar_left").DefaultWidgets.Add("common.admin.widgets/user_login_info");
			areaManager.GetArea("header_navbar_right").DefaultWidgets.Add("common.admin.widgets/enter_admin_panel");
			areaManager.GetArea("admin_navbar").DefaultWidgets.Add("common.admin.widgets/admin_apps_menu");
			areaManager.GetArea("admin_footer_area").DefaultWidgets.Add("common.base.widgets/copyright");
		}
	}
}
