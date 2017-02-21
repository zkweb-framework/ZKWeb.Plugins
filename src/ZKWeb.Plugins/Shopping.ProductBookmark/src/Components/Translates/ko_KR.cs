using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductBookmark.src.Components.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>() {
			{ "ProductBookmark", "제품 컬렉션" },
			{ "Product bookmark feature for ec site", "제품 컬렉션 쇼핑몰 사이트 사용을 특징으로" },
			{ "Bookmark this product", "제품을 수집" },
			{ "Product bookmarked", "제품 즐겨 찾기왔다" },
			{ "Bookmark product require login, redirecting to login page...", "로그인 할 좋아하는 필요성은 로그인 페이지로 이동하는 것입니다……" },
			{ "Product already bookmarked", "제품 즐겨 찾기왔다" },
			{ "Bookmark product success", "즐겨 찾기 성공" },
			{ "BookmarkTime", "시간 즐겨 찾기" },
			{ "ProductAddBookmark", "즐겨 찾기 버튼" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
