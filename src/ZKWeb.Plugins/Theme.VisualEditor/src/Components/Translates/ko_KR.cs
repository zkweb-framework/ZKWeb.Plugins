using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "VisualThemeEditor", "비주얼 편집기" },
			{ "VisualEditor", "비주얼 편집기" },
			{ "Allow edit website theme visually", "그것은 시각적 편집 사이트 테마를 할 수 있습니다" },
			{ "AddElement", "요소를 추가합니다" },
			{ "ManageTheme", "관리 항목" },
			{ "SwitchPage", "스위치 페이지" },
			{ "SaveChanges", "변경 사항을 저장" },
			{ "Please click the page link you want to switch to", "당신이 페이지로 전환 할 링크를 클릭하십시오" },
			{ "Make sure you have saved all the changes, otherwise they will be lost.",
				"당신이 그렇지 않으면 변경 사항이 손실됩니다, 모든 변경 사항을 저장했는지 확인." },
			{ "EnterVisualEditor", "비주얼 편집기에" },
			{ "NoDescription", "설명이 없습니다" },
			{ "RemoveElement", "요소를 제거" },
			{ "Are you sure to remove $element?", "삭제 확인$element?" },
			{ "Add Element Success", "요소를 추가하면 성공" },
			{ "Remove Element Success", "요소의 성공을 제거" },
			{ "Edit Element Success", "편집 요소의 성공" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
