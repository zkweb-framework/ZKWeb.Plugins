using ZKWeb.Plugins.Common.Base.src.Components.GenericConfigs;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Web;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Base.src.Components.HttpRequestHandlers {
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
