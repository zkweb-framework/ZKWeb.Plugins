using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Model;

namespace ZKWeb.Plugins.Common.Admin.src {
	/// <summary>
	/// 载入插件时的处理
	/// </summary>
	[ExportMany]
	public class Plugin : IPlugin {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin() {
			// 注册默认模块
			var diyManager = Application.Ioc.Resolve<DiyManager>();
			diyManager.GetArea("header_navbar_left").DefaultWidgets.Add("common.admin.widgets/user_login_info");
		}
	}
}
