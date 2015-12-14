using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Model;
using ZKWeb.Plugins.Common.Base.src.Config;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Base.src.Controllers {
	/// <summary>
	/// 设置语言和时区
	/// 注意这里设置的语言和时区只能应用于更早注册的处理器
	/// 如果其他插件使用IHttpRequestHandler返回网页将不受这个处理器的影响
	/// </summary>
	[ExportMany]
	public class ApplicationBeginRequestSetLocaleCallback : IHttpRequestHandler {
		/// <summary>
		/// 处理请求
		/// </summary>
		public void OnRequest() {
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var localeSettings = configManager.GetData<LocaleSettings>();
			LocaleUtils.SetThreadLanguageAutomatic(
				localeSettings.AllowDetectLanguageFromBrowser, localeSettings.DefaultLanguage);
			LocaleUtils.SetThreadTimezoneAutomatic(localeSettings.DefaultTimezone);
		}
	}
}
