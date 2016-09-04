using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Order", "주문 용지" },
			{ "OrderManage", "주문 관리" },
			{ "Order management for ec site", "쇼핑몰 사이트는 순서 관리 기능을 이용" },
			{ "Buynow", "지금 주문" },
			{ "AddToCart", "장바구니에 추가" },
			{ "Cart", "카트" },
			{ "The product you are try to purchase does not exist.",
				"당신이 제품이 존재하지 않습니다 구입하려고" },
			{ "The product you are try to purchase does not purchasable.",
				"당신이 구입하려고하는 제품을 구매할하지 않습니다" },
			{ "Order count must larger than 0", "주문 횟수해야보다 큰 0" },
			{ "OrderSettings", "주문 설정" },
			{ "BuynowCartProductExpiresDays", "지금 주문 장바구니 제품 일 만료" },
			{ "NormalCartProductExpiresDays", "일반 장바구니 제품 일 만료" },
			{ "AutoConfirmOrderAfterDays", "일 후 자동 확인 주문" },
			{ "AllowAnonymousVisitorCreateOrder", "익명 방문자가 오더 생성 허용" },
			{ "Create order require user logged in", "순서는 사용자가 로그인을 필요로 만들기" },
			{ "ProductUnitPrice", "제품 단가" },
			{ "ProductTotalPrice", "제품 총 가격" },
			{ "LogisticsCost", "물류 비용" },
			{ "Create order contains multi currency is not supported",
				"성 순서는 다중 통화가 지원되지 않습니다 포함" },
			{ "Add product to cart success", "카트 성공에 제품 추가" },
			{ "Total products", "전체 제품" },
			{ "Product total price", "제품 총 가격" },
			{ "Close", "닫기" },
			{ "Checkout >>>", "점검 >>>" },
			{ "Order product unit price must not be negative", "주문 제품 단가가 음수가 아니어야합니다" },
			{ "Order cost must larger than 0", "주문 비용을해야보다 큰 0" },
			{ "Cart <em>[0]</em> products", "카트 <em>[0]</em>제품" },
			{ "<em>[0]</em> Products", "전부<em>[0]</em>개 제품" },
			{ "Total <em>[0]</em>", "전부<em>[0]</em>" },
			{ "Cart is empty", "빨리 쇼핑 카트 아직 상품 및 구입！" },
			{ "Recently add to cart", "최근 장바구니에 담기" },
			{ "Delete Successfully", "성공적으로 삭제" },
			{ "OrderList", "주문 목록" },
			{ "ShippingAddress", "배송 주소" },
			{ "ShippingAddress", "배송 주소" },
			{ "Address/Name/Tel", "주소/이름/전화" },
			{ "ZipCode", "우편 번호" },
			{ "DetailedAddress", "주소" },
			{ "Fullname", "전체 이름" },
			{ "TelOrMobile", "전화/모바일" },
			{ "SubmitOrder", "주문을 제출" },
			{ "Calculating...", "계산하는..." },
			{ "Products total count: <em>0</em>", "항목의 총 수: <em>0</em>" },
			{ "Products total price: <em>0</em>", "상품 총 가격: <em>0</em>" },
			{ "Shipping Address:", "배송 주소:" },
			{ "Use new address", "새 주소" },
			{ "Manage shipping address", "배송 주소 관리" },
			{ "Check this will save or add shipping address",
				"기존 주소를 저장하거나 새 주소를 추가하려면 여기를 확인" },
			{ "Save shipping address", "저장 배송 주소" },
			{ "OrderComment", "주문 메시지" },
			{ "Logistics:", "물류 배송:" },
			{ "Logistics([0]):", "물류 배송([0]):" },
			{ "PaymentApi:", "지불 게이트웨이:" },
			{ "OrderTransaction", "주문 트랜잭션" },
			{ "Order Comment:", "주문 메시지:" },
			{ "Please select the products you want to purchase", "당신이 구입하려는 항목을 선택하세요" },
			{ "Selected logistics is not available for this seller", "판매자 선택된 스트림을 사용할 수 없다" },
			{ "Please select logistics", "물류를 선택" },
			{ "Please select payment api", "선택 결제 인터페이스" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
