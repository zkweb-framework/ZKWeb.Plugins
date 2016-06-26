using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Order", "注文" },
			{ "OrderManage", "注文管理" },
			{ "Order management for ec site", "オンラインショッピングサイトが使う注文管理機能" },
			{ "Buynow", "今すぐ買う" },
			{ "AddToCart", "カートに入れる" },
			{ "Cart", "カート" },
			{ "The product you are try to purchase does not exist.",
				"購入しようとする商品は存在しません。" },
			{ "The product you are try to purchase does not purchasable.",
				"購入しようとする商品は現在は購入できません" },
			{ "Order count must larger than 0", "注文数は０以上でなければなりません" },
			{ "OrderSettings", "注文設定" },
			{ "BuynowCartProductExpiresDays", "すぐに買うカート内の商品の有効日数" },
			{ "NormalCartProductExpiresDays", "普通のカート内の商品の有効日数" },
			{ "AutoConfirmOrderAfterDays", "注文の自動確認日数" },
			{ "AllowAnonymousVisitorCreateOrder", "匿名利用者からの注文を許可する" },
			{ "Create order require user logged in", "注文するためにはログインが必要" },
			{ "ProductUnitPrice", "商品の単価" },
			{ "ProductTotalPrice", "商品の合計価格" },
			{ "LogisticsCost", "送料" },
			{ "Create order contains multi currency is not supported",
				"一つ以上の通貨が使われる注文を作成することはできません" },
			{ "Add product to cart success",
				"商品をカートに入れました" },
			{ "Total products", "合計商品数" },
			{ "Product total price", "商品の合計価格" },
			{ "Close", "关闭" },
			{ "Checkout >>>", "チェックアウト >>>" },
			{ "Order product unit price must not be negative", "商品の単価は負であってはなりません" },
			{ "Order cost must larger than 0", "注文の価格は０以上でなければなりません" },
			{ "Cart <em>[0]</em> products", "カート<em>[0]</em>件" },
			{ "<em>[0]</em> Products", "合計商品数<em>[0]</em>" },
			{ "Total <em>[0]</em>", "合計<em>[0]</em>" },
			{ "Cart is empty", "カートは空です、商品を入れてください。" },
			{ "Recently add to cart", "最近カートに追加した商品" },
			{ "Delete Successfully", "削除に成功しました" },
			{ "OrderList", "注文リスト" },
			{ "ShippingAddress", "お届け先の住所" },
			{ "UserShippingAddress", "お届け先の住所" },
			{ "Address/Name/Tel", "アドレス/名前/電話" },
			{ "ZipCode", "郵便番号" },
			{ "DetailedAddress", "詳細のアドレス" },
			{ "Fullname", "フルネーム" },
			{ "TelOrMobile", "電話/携帯電話" },
			{ "SubmitOrder", "注文する" },
			{ "Calculating...", "計算中..." },
			{ "Products total count: <em>0</em>", "商品数合計: <em>0</em>" },
			{ "Products total price: <em>0</em>", "商品価格合計: <em>0</em>" },
			{ "Shipping Address:", "お届け先の住所:" },
			{ "Use new address", "新しいアドレスを使う" },
			{ "Manage shipping address", "お届け先の住所を管理する" },
			{ "Check this will save or add shipping address",
				"ここをチェックすると入力した住所を保存できます" },
			{ "Save shipping address", "お届け先の住所を保存する" },
			{ "OrderComment", "注文コメント" },
			{ "Logistics:", "配送方法:" },
			{ "Logistics([0]):", "配送方法([0]):" },
			{ "PaymentApi:", "支払方法:" },
			{ "OrderTransaction", "注文取引" },
			{ "Order Comment:", "注文コメント:" },
			{ "Please select the products you want to purchase", "購入したい商品を選択してください" },
			{ "Selected logistics is not available for this seller", "この販売者は選択した配送方法を利用できません" },
			{ "Please select logistics", "配送方法を選択してください" },
			{ "Please select payment api", "支払方法を選択してください" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
