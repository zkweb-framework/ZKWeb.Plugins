using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugin.Interfaces;
using ZKWeb.Plugins.Common.Captcha.src.Managers;

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
		}
	}
}
