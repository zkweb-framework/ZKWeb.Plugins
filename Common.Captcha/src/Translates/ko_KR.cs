using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Captcha.src.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Captcha", "코드" },
			{ "Click to change captcha image", "확인 코드 이미지를 대체 하려면 클릭" },
			{ "Please enter captcha", "제발 입력 검증 코드" },
			{ "Incorrect captcha", "코드 오류를 확인, 입력 해 주세요는" },
			{ "Provide captcha form field and validation", "인증 코드 양식 필드 및 검증" },
			{ "Captcha Audio", "음성 인증 코드" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
