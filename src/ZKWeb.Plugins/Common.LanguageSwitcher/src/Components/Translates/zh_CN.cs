using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.LanguageSwitcher.src.Components.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Language", "语言" },
			{ "Language Switcher", "语言切换器" },
			{ "Provide manually language switch menu for visitor", "提供给访问者手动切换语言的菜单" },
			{ "LanguageSwitcherSettings", "语言切换设置" },
			{ "SwitchableLanguages", "可切换到的语言" },
			{ "Switch Language", "切换语言" },
			{ "DisplayLanguageSwitcherOnFrontPages", "在前台页面显示语言切换器" },
			{ "DisplayLanguageSwitcherOnAdminPanel", "在后台页面显示语言切换器" },
			// TODO: 翻译到其他语言
			{ "LanguageSwitchMenu", "语言切换菜单" },
			{ "LanguageSwitchMenuForAdminPanel", "后台语言切换菜单" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
