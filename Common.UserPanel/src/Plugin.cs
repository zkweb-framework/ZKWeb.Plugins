using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugin.Interfaces;
using ZKWeb.Plugins.Common.UserPanel.src.Model;
using ZKWeb.Templating.AreaSupport;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.UserPanel.src {
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
			var navbarRight = areaManager.GetArea("header_navbar_right");
			navbarRight.DefaultWidgets.AddBefore("", "common.user_panel.widgets/enter_user_panel");
		}
	}
}
