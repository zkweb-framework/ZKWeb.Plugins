using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.Components.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>() {
			{ "PesudoStatic", "의사 정적" },
			{ "Pesudo static support", "의사 정적 지원" },
			{ "PesudoStaticSettings", "의사 정적 설정" },
			{ "EnablePesudoStatic", "의사 정적 사용" },
			{ "PesudoStaticExtension", "의사 정적 확장" },
			{ "PesudoStaticParamDelimiter", "의사 정적 구문 분석 구분" },
			{ "PesudoStaticPolicy", "의사 정적 정책" },
			{ "BlackListPolicy", "블랙리스트 정책" },
			{ "WhiteListPolicy", "화이트리스트 정책" },
			{ "IncludeUrlPaths", "URL 경로 포함" },
			{ "ExcludeUrlPaths", "URL 경로 제외" },
			{ "One path per line, only available for whitelist policy",
				"한 줄에 하나의 경로, 화이트리스트 정책에만 사용할 수" },
			{ "One path per line, only available for blacklist policy",
				"한 줄에 하나의 경로, 블랙리스트 정책에만 사용할 수" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
