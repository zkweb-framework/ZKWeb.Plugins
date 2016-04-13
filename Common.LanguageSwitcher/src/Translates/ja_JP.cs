using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.LanguageSwitcher.src.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Language", "言語" },
			{ "Language Switcher", "言語切替" },
			{ "Provide manually language switch menu for visitor", "手動で言語を切り替えられるメニューを訪問者に提供する" },
			{ "LanguageSwitcherSettings", "言語切替設定" },
			{ "SwitchableLanguages", "切り替えできる言語" },
			{ "Switch Language", "言語切替" },
			{ "DisplayLanguageSwitcherOnFrontPages", "フロントページで言語切替を表示する" },
			{ "DisplayLanguageSwitcherOnAdminPanel", "管理者パネルで言語切替を表示する" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
