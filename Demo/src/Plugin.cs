using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugin.Interfaces;
using ZKWeb.Plugins.Common.Captcha.src.Managers;
using ZKWeb.Templating.AreaSupport;

namespace ZKWeb.Plugins.Demo.src {
	/// <summary>
	/// 载入插件时的处理
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Plugin : IPlugin {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin() {
			// 当前DEMO环境不支持TTS语音，手动设置不支持
			var captchaManager = Application.Ioc.Resolve<CaptchaManager>();
			captchaManager.SupportCaptchaAudio = false;
			// 注册默认模块
			var areaManager = Application.Ioc.Resolve<TemplateAreaManager>();
			areaManager.GetArea("header_menubar").DefaultWidgets.Add("demo.widgets/demo_nav_menu");
			areaManager.GetArea("index_top_area_1").DefaultWidgets.Add("demo.widgets/demo_index");
		}
	}
}
