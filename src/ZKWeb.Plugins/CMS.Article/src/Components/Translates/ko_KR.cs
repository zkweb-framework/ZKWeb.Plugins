using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.Article.src.Components.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Article", "기사" },
			{ "ArticleManage", "문서 관리" },
			{ "Article management and display", "문서 관리 및 표시" },
			{ "Title/Summary/Author", "표제/개요/저자" },
			{ "ArticleClass", "제 카테고리" },
			{ "ArticleTag", "기술 자료 태그" },
			{ "Title", "표제" },
			{ "Author", "저자" },
			{ "Summary", "개요" },
			{ "ArticleList", "기사 목록" },
			{ "AllArticles", "전체 기사" },
			{ "Posted on", "에 게시 됨" },
			{ "Read More", "자세히보기" },
			{ "No matching articles found, please change the condition and search again.",
				"찾지 일치 기사, 조건을 변경하지 않고 다시 검색하시기 바랍니다."},
			{ "The article you are visiting does not exist.", "존재하지 않는 방문 기사" },
			{ "Preview", "시사" },
			{ "ArticleSettings", "기사 설정" },
			{ "ArticleListSettings", "제리스트 세트" },
			{ "ArticlesPerPage", "페이지 당 기사의 수" },
			{ "CMS", "콘텐츠 관리자" },
			{ "ArticleViewPage", "제 세부 정보 페이지" },
			{ "ArticleListPage", "기사 목록" },
			{ "ArticleContents", "기사 내용" },
			{ "ArticleListNav", "페이지 탐색 기사" },
			{ "ArticleListTable", "기사 목록 페이지 양식" },
			{ "ArticleNavMenu", "게시물 네비게이션 메뉴" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
