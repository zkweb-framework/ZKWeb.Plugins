using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Order", "订单" },
			{ "OrderManage", "订单管理" },
			{ "Order management for ec site", "商城使用的订单管理功能" },
			{ "Buynow", "立刻购买" },
			{ "AddToCart", "加入购物车" },
			{ "Cart", "购物车" },
			{ "The product you are try to purchase does not exist.", "您尝试购买的商品不存在" },
			{ "The product you are try to purchase does not purchasable.", "您尝试购买的商品目前不允许购买" },
			{ "Order count must larger than 0", "订购数量必须大于0" },
			{ "OrderSettings", "订单设置" },
			{ "BuynowCartProductExpiresDays", "立刻购买的购物车商品的过期天数" },
			{ "NormalCartProductExpiresDays", "一般的购物车商品的过期天数" },
			{ "AutoConfirmOrderAfterDays", "自动确认收货天数" },
			{ "AllowAnonymousVisitorCreateOrder", "允许非会员下单" },
			{ "Create order require user logged in", "创建订单需要用户登录" },
			{ "ProductUnitPrice", "商品单价" },
			{ "ProductTotalPrice", "商品总价" },
			{ "LogisticsCost", "运费" },
			{ "Create order contains multi currency is not supported", "不支持创建包含多种货币的订单" },
			{ "Add product to cart success", "成功添加商品到购物车" },
			{ "Total products", "共有商品" },
			{ "Product total price", "商品总价" },
			{ "Close", "关闭" },
			{ "Checkout >>>", "去结算 >>>" },
			{ "Order product unit price must not be negative", "订单商品的单价必须大于或等于0" },
			{ "Order cost must larger than 0", "订单总价必须大于0" },
			{ "Cart <em>[0]</em> products", "购物车<em>[0]</em>件" },
			{ "<em>[0]</em> Products", "共<em>[0]</em>件商品" },
			{ "Total <em>[0]</em>", "共<em>[0]</em>" },
			{ "Cart is empty", "购物车中还没有商品，赶紧选购吧！" },
			{ "Recently add to cart", "最近加入购物车" },
			{ "Delete Successfully", "删除成功" },
			{ "OrderList", "订单列表" },
			{ "ShippingAddress", "收货地址" },
			{ "UserShippingAddress", "收货地址" },
			{ "Address/Name/Tel", "地址/姓名/电话" },
			{ "ZipCode", "邮政编码" },
			{ "DetailedAddress", "详细地址" },
			{ "Fullname", "姓名" },
			{ "TelOrMobile", "电话/手机" },
			{ "SubmitOrder", "提交订单" },
			{ "Calculating...", "计算中..." },
			{ "Products total count: <em>0</em>", "商品总数量: <em>0</em>" },
			{ "Products total price: <em>0</em>", "商品总价格: <em>0</em>" },
			{ "Shipping Address:", "收货地址:" },
			{ "Use new address", "使用新地址" },
			{ "Manage shipping address", "管理收货地址" },
			{ "Check this will save or add shipping address", "勾选这里可以保存现有地址或添加新的地址" },
			{ "Save shipping address", "保存收货地址" },
			{ "OrderComment", "订单留言" },
			{ "Logistics:", "物流配送:" },
			{ "Logistics([0]):", "物流配送([0]):" },
			{ "PaymentApi:", "支付接口:" },
			{ "OrderTransaction", "订单交易" },
			{ "Order Comment:", "订单留言:" },
			{ "Please select the products you want to purchase", "请选择您想要购买的商品" },
			{ "Selected logistics is not available for this seller", "该卖家不可使用选择的物流" },
			{ "Please select logistics", "请选择物流" },
			{ "Please select payment api", "请选择支付接口" },
			// TODO: 添加翻译
			{ "Order contains product that not exist or deleted", "订单包含不存在或已删除的商品" },
			{ "Insufficient stock of product [{0}]", "商品[{0}]的库存不足" },
			{ "Order contains real products, please select a logistics", "订单包含实体商品，请选择一个物流" },
			{ "Please provide detailed address", "请提供详细的地址" },
			{ "Please provide receiver name", "请提供收货人姓名" },
			{ "Please provide receiver tel or mobile", "请提供收货人电话或手机" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
