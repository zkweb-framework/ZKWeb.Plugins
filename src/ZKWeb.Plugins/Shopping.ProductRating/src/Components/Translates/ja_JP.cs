using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductRating.src.Components.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>() {
			{ "ProductRating", "商品レビュー" },
			{ "Order product rating feature for ec site", "ECサイトの商品レビュー機能" },
			{ "Rate", "評価" },
			{ "OrderRating", "注文のレビュー" },
			{ "RateNextTime", "次回で評価する" },
			{ "GoodRate", "良い" },
			{ "MediumRate", "普通" },
			{ "BadRate", "悪い" },
			{ "Please fill in your evaluation of this product", "商品の評価を記入してください" },
			{ "No products available for rating, Please return to order list", "評価できる商品がありません、注文リストに戻ってください" },
			{ "DescriptionMatchScore", "説明一致のスコア" },
			{ "ServiceQualityScore", "サービス品質のスコア" },
			{ "DeliverySpeedScore", "配達速度のスコア" },
			{ "Please provide description match score", "説明一致のスコアを記載してください" },
			{ "Please provide service quality score", "サービス品質のスコアを記載してください" },
			{ "Please provide delivery speed score", "配達速度のスコアを記載してください" },
			{ "Invalid description match score", "説明一致のスコアが不正です" },
			{ "Invalid service quality score", "サービス品質のスコアが不正です" },
			{ "Invalid delivery speed score", "配達速度のスコアが不正です" },
			{ "Invalid order state for rating", "評価できない状態の注文です" },
			{ "Please provide rating for atleast one product", "少なくとも一つの商品の評価を提供してください" },
			{ "Rating successful, Redirecting to order list...", "評価が成功しました、注文リストにリダイレクトします……" },
			{ "OrderRatePage", "注文の評価ページ" },
			{ "ProductRatingHistory", "商品評価の記録" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
