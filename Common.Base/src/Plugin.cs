using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Model;

namespace ZKWeb.Plugins.Common.Base.src {
	/// <summary>
	/// 载入插件时的处理
	/// </summary>
	[ExportMany]
	public class Plugin : IPlugin {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin() {
			// 初始化定时任务管理器
			Application.Ioc.Resolve<ScheduledTaskManager>();
			// 注册默认模块
			var diyManager = Application.Ioc.Resolve<DiyManager>();
			diyManager.GetArea("test_area").DefaultWidgets.Add("common.base.logo");
		}
	}
}
