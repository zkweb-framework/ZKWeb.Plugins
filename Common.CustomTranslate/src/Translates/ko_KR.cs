using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "CustomTranslate", "사용자 지정 번역" },
			{ "Support custom translate through admin panel", "관리자는 사용자 지정 변환 설정에서 지원" },
			{ "Translation", "번역 된 콘텐츠" },
			{ "Origin/Translated", "소스/대상" },
			{ "OriginalText", "원래 텍스트" },
			{ "TranslatedText", "번역" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
