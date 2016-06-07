using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.LanguageSwitcher.src.Config;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;
using ZKWeb.Utils.IocContainer;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web.Interfaces;

namespace ZKWeb.Plugins.Common.LanguageSwitcher.src.Controllers {
	/// <summary>
	/// Api控制器
	/// </summary>
	[ExportMany]
	public class ApiController : IController {
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
		public IActionResult SwitchToLanguage() {
			var language = HttpContextUtils.CurrentContext.Request.Get<string>("language");
			HttpContextUtils.PutCookie(LocaleUtils.LanguageKey, language);
			return new JsonResult(new { script = ScriptStrings.RefreshAfter(0) });
		}
	}
}
