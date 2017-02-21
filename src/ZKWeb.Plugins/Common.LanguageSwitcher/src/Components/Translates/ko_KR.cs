using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.LanguageSwitcher.src.Components.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Language", "언어" },
			{ "Language Switcher", "언어 스위처" },
			{ "Provide manually language switch menu for visitor", "방문자를위한 수동으로 언어 스위치 메뉴를 제공" },
			{ "LanguageSwitcherSettings", "언어 설정 변경" },
			{ "SwitchableLanguages", "전환 언어" },
			{ "Switch Language", "스위치 언어" },
			{ "DisplayLanguageSwitcherOnFrontPages", "첫 페이지에서 언어 전환" },
			{ "DisplayLanguageSwitcherOnAdminPanel", "배경 페이지에서 언어 전환" },
			{ "LanguageSwitchMenu", "언어 변경" },
			{ "LanguageSwitchMenuForAdminPanel", "근원 전환 선택" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
