using System;
using System.Linq;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderCheckers.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderHandlers.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Product.src.Components.ProductTypes.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions {
	/// <summary>
	/// 卖家订单的扩展函数
	/// </summary>
	public static class SellerOrderExtensions {
		/// <summary>
		/// 检查函数的类型
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="result">判断结果</param>
		public delegate void CheckFunc(SellerOrder order, ref Pair<bool, string> result);

		/// <summary>
		/// 检查订单是否满足指定条件
		/// 返回是否满足和文本信息
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="getCheckFunc">获取检查函数的函数</param>
		/// <returns></returns>
		public static Pair<bool, string> Check(
			this SellerOrder order, Func<IOrderChecker, CheckFunc> getCheckFunc) {
			var result = Pair.Create(false, "No Result");
			var checkers = Application.Ioc.ResolveMany<IOrderChecker>();
			checkers.ForEach(c => getCheckFunc(c)(order, ref result));
			return result;
		}

		/// <summary>
		/// 设置订单的状态和状态时间
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="state">状态</param>
		public static void SetState(
			this SellerOrder order, OrderState state) {
			// 设置状态和时间
			order.State = state;
			order.StateTimes[state] = DateTime.UtcNow;
			// 触发setter
			order.StateTimes = order.StateTimes;
			// 通知状态改变
			foreach (var handler in Application.Ioc.ResolveMany<IOrderHandler>()) {
				handler.OnStateChanged(order, state);
			}
		}

		/// <summary>
		/// 判断订单是否包含实体商品
		/// </summary>
		/// <param name="order">订单</param>
		/// <returns></returns>
		public static bool ContainsRealProduct(this SellerOrder order) {
			return order.OrderProducts.Any(p => p.Product.GetProductType() is IAmRealProduct);
		}
	}
}
