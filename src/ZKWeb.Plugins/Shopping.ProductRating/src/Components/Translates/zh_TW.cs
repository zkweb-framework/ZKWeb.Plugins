using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductRating.src.Components.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>() {
			{ "ProductRating", "商品評價" },
			{ "Order product rating feature for ec site", "商城網站使用的商品評價功能" },
			{ "Rate", "評價" },
			{ "OrderRating", "訂單評價" },
			{ "RateNextTime", "下次評價" },
			{ "GoodRate", "好評" },
			{ "MediumRate", "中評" },
			{ "BadRate", "差評" },
			{ "Please fill in your evaluation of this product", "請填寫您對此商品的評價" },
			{ "No products available for rating, Please return to order list", "沒有可評價的商品，請返回訂單列表" },
			{ "DescriptionMatchScore", "描述相符評分" },
			{ "ServiceQualityScore", "服務質量評分" },
			{ "DeliverySpeedScore", "發貨速度評分" },
			{ "Please provide description match score", "請對描述相符進行評分" },
			{ "Please provide service quality score", "請對服務質量進行評分" },
			{ "Please provide delivery speed score", "請對發貨速度進行評分" },
			{ "Invalid description match score", "不正確的描述相符評分" },
			{ "Invalid service quality score", "不正確的服務質量評分" },
			{ "Invalid delivery speed score", "不正確的發貨速度評分" },
			{ "Invalid order state for rating", "不正確的訂單狀態，不能評價此訂單" },
			{ "Please provide rating for atleast one product", "請對至少壹件商品進行評價" },
			{ "Rating successful, Redirecting to order list...", "評價成功，正在跳轉到訂單列表頁……" },
			{ "OrderRatePage", "訂單評價頁" },
			{ "ProductRatingHistory", "商品評價記錄" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
