using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductBookmark.src.Components.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>() {
			{ "ProductBookmark", "お気に入り商品" },
			{ "Product bookmark feature for ec site", "ECサイトのお気に入り商品機能" },
			{ "Bookmark this product", "お気に入り商品追加" },
			{ "Product bookmarked", "お気に入り商品です" },
			{ "Bookmark product require login, redirecting to login page...", "商品をブックマークするにはログインが必要です、ログインページにリダイレクトします……" },
			{ "Product already bookmarked", "既にお気に入り商品です" },
			{ "Bookmark product success", "お気に入り商品の追加成功しました" },
			{ "BookmarkTime", "ブックマーク時刻" },
			{ "ProductAddBookmark", "お気に入りボタン" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
