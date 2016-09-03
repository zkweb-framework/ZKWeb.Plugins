using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.Article.src.Components.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Article", "文章" },
			{ "ArticleManage", "文章管理" },
			{ "Article management and display", "文章管理と表示機能" },
			{ "Title/Summary/Author", "タイトル/概要/作者" },
			{ "ArticleClass", "文章クラス" },
			{ "ArticleTag", "文章タグ" },
			{ "Title", "タイトル" },
			{ "Author", "作者" },
			{ "Summary", "概要" },
			{ "ArticleList", "文章リスト" },
			{ "AllArticles", "文章一覧" },
			{ "Posted on", "発表時刻" },
			{ "Read More", "続きを読む" },
			{ "No matching articles found, please change the condition and search again.",
				"条件に合った文章が見つかりません、条件を変えて再度検索してください。"},
			{ "The article you are visiting does not exist.", "ご覧の文章は存在しません。" },
			{ "Preview", "プレビュー" },
			{ "ArticleSettings", "文章設定" },
			{ "ArticleListSettings", "文章リスト設定" },
			{ "ArticlesPerPage", "ページごとに表示する文章数" },
			{ "CMS", "コンテンツ管理" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
