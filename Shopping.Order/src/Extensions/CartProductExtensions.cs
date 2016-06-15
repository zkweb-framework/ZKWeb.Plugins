using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Shopping.Order.src.Database;
using ZKWeb.Plugins.Shopping.Order.src.Model;

namespace ZKWeb.Plugins.Shopping.Order.src.Extensions {
	/// <summary>
	/// 购物车商品的扩展函数
	/// </summary>
	public static class CartProductExtensions {
		/// <summary>
		/// 从购物车商品构建订单商品创建参数
		/// </summary>
		/// <param name="cartProduct">购物车商品</param>
		/// <returns></returns>
		public static CreateOrderProductParameters ToCreateOrderProductParameters(
			this CartProduct cartProduct) {
			var parameters = new CreateOrderProductParameters();
			parameters.ProductId = cartProduct.Product.Id;
			parameters.MatchParameters = cartProduct.MatchParameters;
			return parameters;
		}

		/// <summary>
		/// 从购物车商品生成显示信息
		/// 生成后会设置购物车商品Id到附加数据
		/// </summary>
		/// <param name="cartProduct">购物车商品</param>
		/// <returns></returns>
		public static OrderProductDisplayInfo ToOrderProductDisplayInfo(
			this CartProduct cartProduct) {
			var parameters = cartProduct.ToCreateOrderProductParameters();
			var userId = cartProduct.Buyer == null ? null : (long?)cartProduct.Buyer.Id;
			var displayInfo = parameters.ToOrderProductDisplayInfo(userId);
			displayInfo.Extra["cartProductId"] = cartProduct.Id;
			return displayInfo;
		}
	}
}
