using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Components.Translates {
	/// <summary>
	/// 韩语翻译
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
			{ "NonSalesProperties", "재산" },
			{ "SalesProperties", "명세서" },
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
			{ "Weight(g)", "무게(그램)" },
			{ "ProductList", "제품 목록" },
			{ "Preview", "시사" },
			{ "The product you are visiting does not exist.", "사용자는 상품이 존재하지 않는 볼" },
			{ "Brand", "브랜드" },
			{ "ModelNumber", "모델 번호" },
			{ "{0:F2} gram", "{0:F2} 그램(g)" },
			{ "{0:F2}~{1:F2} gram", "{0:F2}~{1:F2} 그램(g)" },
			{ "BestSales", "최고의 판매" },
			{ "LowerPrice", "낮은 가격" },
			{ "HigherPrice", "높은 가격" },
			{ "NewestOnSale", "새로운 도착" },
			{ "FilterByPrice", "가격 필터" },
			{ "ProductListSettings", "제품 목록 설정" },
			{ "ProductsPerPage", "페이지 당 표시되는 항목의 수" },
			{ "Please enter product name", "제품 이름을 입력하세요" },
			{ "All Products", "모든 제품" },
			{ "No matching products found, please change the condition and search again.",
				"일치하는 제품을 찾을 수는 조건을 변경하지 않고 다시 검색하시기 바랍니다."},
			{ "ProductProperty", "상용 부동산" },
			{ "ProductPropertyManage", "상용 부동산 관리" },
			{ "ProductCategory", "제품 카테고리" },
			{ "ProductCategoryManage", "제품 카테고리 관리" },
			{ "IsSalesProperty", "부동산 판매" },
			{ "PropertyValues", "속성 값" },
			{ "ControlType", "제어 유형" },
			{ "TextBox", "텍스트 상자" },
			{ "CheckBox", "확인란" },
			{ "RadioButton", "단일 상자" },
			{ "DropdownList", "드롭 다운 목록을" },
			{ "EditableDropdownList", "드롭 다운 목록을 편집 할 수 있습니다" },
			{ "OrderCount", "수량" },
			{ "Product is OnSale", "제품 판매에있다" },
			{ "Product is StopSelling", "제품 중지 판매" },
			{ "Product is WaitForSales", "제품 판매를 기다릴 것입니다" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
