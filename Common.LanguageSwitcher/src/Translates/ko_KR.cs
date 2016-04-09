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
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Language Switcher", "언어 스위처" },
			{ "Provide manually language switch menu for visitor", "방문자를위한 수동으로 언어 스위치 메뉴를 제공" },
			{ "LanguageSwitcherSettings", "언어 설정 변경" },
			{ "SwitchableLanguages", "전환 언어" },
			{ "Switch Language", "스위치 언어" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
