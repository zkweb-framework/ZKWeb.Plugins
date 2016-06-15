using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.AdminSettings.src.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "AdminSettings", "배경으로 설정" },
			{ "Centralized management for admin settings", "제공 배경 설정의 중앙된 관리" },
			{ "Settings", "설정" },
			{ "BaseSettings", "기본 설정" },
			{ "WebsiteSettings", "사이트" },
			{ "LocaleSettings", "언어 설정" },
			{ "Please click the option you want to change on sidebar", "사이드바에서 수정 하려는 설정을 선택" },
			{ "WebsiteName", "사이트 이름" },
			{ "DocumentTitleFormat", "제목 서식" },
			{ "Default is {title} - {websiteName}", "기본값은 {title} - {websiteName}"},
			{ "CopyrightText", "저작권 정보" },
			{ "DefaultLanguage", "기본 언어" },
			{ "DefaultTimezone", "기본 시간대" },
			{ "AllowDetectLanguageFromBrowser", "브라우저에서 언어 감지 허용" },
			{ "ar-DZ", "아랍어" },
			{ "zh-CN", "중국어" },
			{ "cs-CZ", "체코" },
			{ "en-US", "영어" },
			{ "fr-FR", "프랑스" },
			{ "de-DE", "독일" },
			{ "el-GR", "그리스 언어" },
			{ "it-IT", "이탈리아 언어" },
			{ "ja-JP", "일본어" },
			{ "ko-KR", "한국" },
			{ "pl-PL", "폴란드 언어" },
			{ "ru-RU", "러시아어" },
			{ "es-ES", "스페인 언어" },
			{ "th-TH", "태국" },
			{ "zh-TW", "중국어 번체" },
			{ "FrontPageLogo", "프론트 페이지 로고" },
			{ "AdminPanelLogo", "관리자 패널 로고" },
			{ "RestoreDefaultFrontPageLogo", "기본 프론트 페이지 로고 복원" },
			{ "RestoreDefaultAdminPanelLogo", "기본 관리자 패널 로고 복원" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
