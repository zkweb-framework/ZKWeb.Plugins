using System;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.LanguageSwitcher.src.Components.GenericConfigs;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ScriptStrings;

namespace ZKWeb.Plugins.Common.LanguageSwitcher.src.Controllers {
	/// <summary>
	/// Api控制器
	/// </summary>
	[ExportMany]
	public class LanguageSwitcherApiController : ControllerBase {
		/// <summary>
		/// 获取可切换的语言列表
		/// </summary>
		/// <returns></returns>
		[Action("api/locale/language_switcher_settings")]
		public IActionResult GetSwitchableLanguages() {
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var settings = configManager.GetData<LanguageSwitcherSettings>();
			return new JsonResult(settings);
		}

		/// <summary>
		/// 切换到指定语言
		/// </summary>
		/// <returns></returns>
		[Action("api/locale/switch_to_language", HttpMethods.POST)]
		public IActionResult SwitchToLanguage(string language) {
			Context.PutCookie(LocaleUtils.LanguageKey, language);
			return new JsonResult(new { script = BaseScriptStrings.RefreshAfter(0) });
		}
	}
}
