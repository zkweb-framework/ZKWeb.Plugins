using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Components.GenericConfigs;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderHandlers.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderHandlers {
	/// <summary>
	/// 库存处理器
	/// </summary>
	[ExportMany]
	public class OrderStockReduceHandler : IOrderHandler {
		/// <summary>
		/// 根据订单状态的改变扣减库存
		/// </summary>
		public void OnStateChanged(SellerOrder order, OrderState state) {
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var orderSettings = configManager.GetData<OrderSettings>();
			if ((state == OrderState.WaitingBuyerPay &&
				orderSettings.StockReductionMode == StockReductionMode.AfterCreateOrder) ||
				(state == OrderState.WaitingSellerDeliveryGoods &&
				orderSettings.StockReductionMode == StockReductionMode.AfterOrderPaid)) {
				var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
				orderManager.ReduceStock(order);
			}
		}
	}
}
