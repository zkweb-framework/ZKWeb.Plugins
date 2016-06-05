using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

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
			// TODO: 以下未翻译到其他语言
			{ "Buynow", "立刻购买" },
			{ "AddToCart", "加入购物车" },
			{ "The product you are try to purchase does not exist.", "您尝试购买的商品不存在" },
			{ "The product you are try to purchase does not purchasable.", "您尝试购买的商品目前不允许购买" },
			{ "Order count must large than 0", "订购数量必须大于0" },
			{ "OrderSettings", "订单设置" },
			{ "BuynowCartProductExpiresDays", "立刻购买的购物车商品的过期天数" },
			{ "NormalCartProductExpiresDays", "一般的购物车商品的过期天数" },
			{ "AutoConfirmOrderAfterDays", "自动确认收货天数" },
			{ "AllowAnonymousVisitorCreateOrder", "允许非会员下单" },
			{ "Create order require user logged in", "创建订单需要用户登录" },
			{ "ProductUnitPrice", "商品单价" },
			{ "ProductTotalPrice", "商品总价" },
			{ "Create order contains multi currency is not supported", "不支持创建包含多种货币的订单" },
			{ "Add product to cart success", "成功添加商品到购物车" },
			{ "Total products", "共有商品" },
			{ "Product total price", "商品总价" },
			{ "Close", "关闭" },
			{ "Checkout >>>", "去结算 >>>" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
