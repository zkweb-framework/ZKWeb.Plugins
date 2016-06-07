using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.LanguageSwitcher.src.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Language", "語言" },
			{ "Language Switcher", "語言切換器" },
			{ "Provide manually language switch menu for visitor", "提供給訪問者手動切換語言的菜單" },
			{ "LanguageSwitcherSettings", "語言切換設置" },
			{ "SwitchableLanguages", "可切換到的語言" },
			{ "Switch Language", "切換語言" },
			{ "DisplayLanguageSwitcherOnFrontPages", "在前端頁面顯示語言切換器" },
			{ "DisplayLanguageSwitcherOnAdminPanel", "在後端頁面顯示語言切換器" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
