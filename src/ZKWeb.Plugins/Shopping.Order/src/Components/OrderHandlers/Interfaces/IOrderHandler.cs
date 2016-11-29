using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderHandlers.Interfaces {
	/// <summary>
	/// 订单处理器
	/// </summary>
	public interface IOrderHandler {
		/// <summary>
		/// 订单状态改变时的处理
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="state">订单状态</param>
		void OnStateChanged(SellerOrder order, OrderState state);
	}
}
