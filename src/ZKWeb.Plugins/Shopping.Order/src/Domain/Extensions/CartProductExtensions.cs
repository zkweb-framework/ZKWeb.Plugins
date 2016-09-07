using System;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions {
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
			parameters.Extra["cartProductId"] = cartProduct.Id;
			return parameters;
		}

		/// <summary>
		/// 更新购物车商品的订购数量
		/// </summary>
		/// <param name="cartProduct">购物车商品</param>
		/// <param name="orderCount">订购数量</param>
		public static void UpdateOrderCount(this CartProduct cartProduct, long orderCount) {
			cartProduct.Count = orderCount;
			cartProduct.MatchParameters.SetOrderCount(orderCount);
		}
	}
}
