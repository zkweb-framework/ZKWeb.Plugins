using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductRating.src.Components.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>() {
			{ "ProductRating", "제품 리뷰" },
			{ "Order product rating feature for ec site", "평가 함수를 이용하여 물품 쇼핑몰 사이트" },
			{ "Rate", "평가" },
			{ "OrderRating", "주문 평가" },
			{ "RateNextTime", "다음 평가" },
			{ "GoodRate", "유리한 댓글" },
			{ "MediumRate", "평균" },
			{ "BadRate", "가난한" },
			{ "Please fill in your evaluation of this product", "이 제품을 평가 평가를 기입하시기 바랍니다" },
			{ "No products available for rating, Please return to order list", "어떤 평가 항목은, 주문 목록으로 돌아가주세요" },
			{ "DescriptionMatchScore", "설명이 일치 점수" },
			{ "ServiceQualityScore", "서비스 평가의 품질" },
			{ "DeliverySpeedScore", "전송 속도 등급" },
			{ "Please provide description match score", "경기 점수를 적어주세요" },
			{ "Please provide service quality score", "품질 등급 서비스주십시오" },
			{ "Please provide delivery speed score", "하십시오 전송 속도 등급" },
			{ "Invalid description match score", "잘못된 설명이 일치 점수" },
			{ "Invalid service quality score", "잘못된 서비스 품질 평가" },
			{ "Invalid delivery speed score", "잘못된 전송 속도 등급" },
			{ "Invalid order state for rating", "잘못된 주문 상태,이 순서는 평가 될 수 없다" },
			{ "Please provide rating for atleast one product", "적어도 하나의 제품에 대한 평가주십시오" },
			{ "Rating successful, Redirecting to order list...", "성공의 평가는 주문 목록 페이지로 이동하는 것입니다……" },
			{ "OrderRatePage", "주문 평가 페이지" },
			{ "ProductRatingHistory", "제품 평가 기록" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
