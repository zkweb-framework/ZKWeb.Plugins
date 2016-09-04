using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Order", "訂單" },
			{ "OrderManage", "訂單管理" },
			{ "Order management for ec site", "商城使用的訂單管理功能" },
			{ "Buynow", "立刻購買" },
			{ "AddToCart", "加入購物車" },
			{ "Cart", "購物車" },
			{ "The product you are try to purchase does not exist.", "您嘗試購買的商品不存在" },
			{ "The product you are try to purchase does not purchasable.", "您嘗試購買的商品目前不允許購買" },
			{ "Order count must larger than 0", "訂購數量必須大於0" },
			{ "OrderSettings", "訂單設置" },
			{ "BuynowCartProductExpiresDays", "立刻購買的購物車商品的過期天數" },
			{ "NormalCartProductExpiresDays", "壹般的購物車商品的過期天數" },
			{ "AutoConfirmOrderAfterDays", "自動確認收貨天數" },
			{ "AllowAnonymousVisitorCreateOrder", "允許非會員下單" },
			{ "Create order require user logged in", "創建訂單需要用戶登錄" },
			{ "ProductUnitPrice", "商品單價" },
			{ "ProductTotalPrice", "商品總價" },
			{ "LogisticsCost", "運費" },
			{ "Create order contains multi currency is not supported", "不支持創建包含多種貨幣的訂單" },
			{ "Add product to cart success", "成功添加商品到購物車" },
			{ "Total products", "共有商品" },
			{ "Product total price", "商品總價" },
			{ "Close", "關閉" },
			{ "Checkout >>>", "去結算 >>>" },
			{ "Order product unit price must not be negative", "訂單商品的單價必須大於或等於0" },
			{ "Order cost must larger than 0", "訂單總價必須大於0" },
			{ "Cart <em>[0]</em> products", "購物車<em>[0]</em>件" },
			{ "<em>[0]</em> Products", "共<em>[0]</em>件商品" },
			{ "Total <em>[0]</em>", "共<em>[0]</em>" },
			{ "Cart is empty", "購物車中還沒有商品，趕緊選購吧！" },
			{ "Recently add to cart", "最近加入購物車" },
			{ "Delete Successfully", "刪除成功" },
			{ "OrderList", "訂單列表" },
			{ "ShippingAddress", "收貨地址" },
			{ "UserShippingAddress", "收貨地址" },
			{ "Address/Name/Tel", "地址/姓名/電話" },
			{ "ZipCode", "郵政編碼" },
			{ "DetailedAddress", "詳細地址" },
			{ "Fullname", "姓名" },
			{ "TelOrMobile", "電話/手機" },
			{ "SubmitOrder", "提交訂單" },
			{ "Calculating...", "計算中..." },
			{ "Products total count: <em>0</em>", "商品總數量: <em>0</em>" },
			{ "Products total price: <em>0</em>", "商品總價格: <em>0</em>" },
			{ "Shipping Address:", "收貨地址:" },
			{ "Use new address", "使用新地址" },
			{ "Manage shipping address", "管理收貨地址" },
			{ "Check this will save or add shipping address", "勾選這裏可以保存現有地址或添加新的地址" },
			{ "Save shipping address", "保存收貨地址" },
			{ "OrderComment", "訂單留言" },
			{ "Logistics:", "物流配送:" },
			{ "Logistics([0]):", "物流配送([0]):" },
			{ "PaymentApi:", "支付接口:" },
			{ "OrderTransaction", "訂單交易" },
			{ "Order Comment:", "訂單留言:" },
			{ "Please select the products you want to purchase", "請選擇您想要購買的商品" },
			{ "Selected logistics is not available for this seller", "該賣家不可使用選擇的物流" },
			{ "Please select logistics", "請選擇物流" },
			{ "Please select payment api", "請選擇支付接口" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
