using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductBookmark.src.Components.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>() {
			// TODO: 添加翻译
			{ "ProductBookmark", "商品收藏" },
			{ "Product bookmark feature for ec site", "商城网站使用的商品收藏功能" },
			{ "Bookmark this product", "收藏本商品" },
			{ "Product bookmarked", "商品已收藏" },
			{ "Bookmark product require login, redirecting to login page...", "收藏商品需要登陆，正在跳转到登录页……" },
			{ "Product already bookmarked", "商品已收藏" },
			{ "Bookmark product success", "收藏商品成功" },
			{ "BookmarkTime", "收藏时间" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
