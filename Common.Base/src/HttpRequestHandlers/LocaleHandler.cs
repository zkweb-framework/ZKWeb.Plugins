using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Base.src.Config;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using ZKWeb.Web;

namespace ZKWeb.Plugins.Common.Base.src.HttpRequestHandlers {
	/// <summary>
	/// 设置语言和时区
	/// </summary>
	[ExportMany, SingletonReuse]
	public class LocaleHandler : IHttpRequestPreHandler {
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
