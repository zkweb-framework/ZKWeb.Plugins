using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Shopping.Product.src.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Product", "상품" },
			{ "ProductManage", "제품 관리자" },
			{ "Product management for ec site", "몰 이용하여 상품 관리" },
			{ "ProductType", "제품 유형" },
			{ "RealProduct", "실제 상품" },
			{ "VirtualProduct", "가상 제품" },
			{ "ProductState", "제품 상태" },
			{ "OnSale", "에 추가" },
			{ "StopSelling", "선반 끄기" },
			{ "WaitForSales", "판매를위한 준비" },
			{ "ProductClass", "범주" },
			{ "ProductTag", "제품 태그" },
			{ "Price", "가격" },
			{ "Stock", "재고" },
			{ "Seller", "판매자" },
			{ "ProductAlbumSettings", "제품 앨범 설정" },
			{ "ProductSettings", "제품 설정" },
			{ "OriginalImageWidth", "원래 폭" },
			{ "OriginalImageHeight", "원래 높이" },
			{ "ThumbnailImageWidth", "미리보기 폭" },
			{ "ThumbnailImageHeight", "미리보기 높이" },
			{ "ProductAlbum", "제품 앨범" },
			{ "ProductProperties", "부동산의 사양" },
			{ "ProductPriceAndStock", "주식 가격" },
			{ "ProductIntroduction", "제품 소개" },
			{ "As Main Image", "도 주로 위치" },
			{ "Clear", "제거" },
			{ "Uploaded pictures will be scaled to [0]x[1], " +
				"upload pictures of this size can achieve the best display effect",
				"현재 업로드 된 이미지는 [0]×[1], 가장 좋은 결과를 얻을 수 있습니다이 크기의 사진을 업로드를 확장 할 것" },
			{ "Category", "범주" },
			{ "Category not exists", "카테고리가 존재하지 않습니다" },
			{ "Non sales properties", "재산" },
			{ "Sales properties", "명세서" },
			{ "Sure to change category? The properties you selected will lost!",
				"카테고리를 수정할 필요성을 인식? 현재 선택된의 사양 및 특성은 손실됩니다！"},
			{ "Condition", "디폴트 값" },
			{ "Default", "디폴트 값" },
			{ "Order count >=", "수량 >=" },
			{ "InhertIfEmpty", "때 상속이 비어" },
			{ "PriceCurrency", "가격 통화" },
			{ "OrderCountGE", "수량 >=" },
			{ "All", "완전한" },
			{ "Weight", "무게" },
			{ "Weight(g)", "무게(그램)" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
