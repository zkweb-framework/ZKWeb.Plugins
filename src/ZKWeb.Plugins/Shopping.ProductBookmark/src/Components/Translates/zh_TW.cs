using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductBookmark.src.Components.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>() {
			{ "ProductBookmark", "商品收藏" },
			{ "Product bookmark feature for ec site", "商城網站使用的商品收藏功能" },
			{ "Bookmark this product", "收藏本商品" },
			{ "Product bookmarked", "商品已收藏" },
			{ "Bookmark product require login, redirecting to login page...", "收藏商品需要登陸，正在跳轉到登錄頁……" },
			{ "Product already bookmarked", "商品已收藏" },
			{ "Bookmark product success", "收藏商品成功" },
			{ "BookmarkTime", "收藏時間" },
			{ "ProductAddBookmark", "收藏商品按鈕" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
