using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductRating.src.Components.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>() {
			// TODO: 添加翻译
			{ "ProductRating", "商品评价" },
			{ "Order product rating feature for ec site", "商城网站使用的商品评价功能" },
			{ "Rate", "评价" },
			{ "OrderRating", "订单评价" },
			{ "RateNextTime", "下次评价" },
			{ "GoodRate", "好评" },
			{ "MediumRate", "中评" },
			{ "BadRate", "差评" },
			{ "Please fill in your evaluation of this product", "请填写您对此商品的评价" },
			{ "No products available for rating, Please return to order list", "没有可评价的商品，请返回订单列表" },
			{ "DescriptionMatchScore", "描述相符评分" },
			{ "ServiceQualityScore", "服务质量评分" },
			{ "DeliverySpeedScore", "发货速度评分" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
