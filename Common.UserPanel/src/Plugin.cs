using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Model;

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
			var diyManager = Application.Ioc.Resolve<DiyManager>();
			var navbarRight = diyManager.GetArea("header_navbar_right");
			navbarRight.DefaultWidgets.AddBefore("", "common.user_panel.widgets/enter_user_panel");
		}
	}
}
