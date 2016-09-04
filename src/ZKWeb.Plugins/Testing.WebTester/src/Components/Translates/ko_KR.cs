using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Testing.WebTester.src.Components.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "WebTester", "웹 테스터 장치" },
			{ "Support running unit test from admin panel", "배경 관리자에서 단위 테스트를 실행하기위한 지원" },
			{ "UnitTest", "단위 테스트" },
			{ "Run", "달리기" },
			{ "Assembly", "조립" },
			{ "Passed", "통과" },
			{ "Skipped", "건너 뜀" },
			{ "Failed", "실패한" },
			{ "ErrorMessage", "에러 메시지" },
			{ "DebugMessage", "디버그 메시지" },
			{ "Start", "스타트" },
			{ "StartAll", "스타 소나무" },
			{ "ResetAll", "모든 재설정" },
			{ "NotRunning", "실행되지 않음" },
			{ "WaitingToRun", "실행 대기" },
			{ "Running", "운전" },
			{ "FinishedRunning", "완료 실행" },
			{ "Getting", "에서 가져 오기" },
			{ "Request submitted, wait processing", "요청 제출, 처리 기다립니다" },
			{ "Function Test", "기능 시험" },
			{ "Run tests", "테스트 실행" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
