using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderHandlers.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderHandlers {
	/// <summary>
	/// 后台通知支付平台已发货的处理器
	/// </summary>
	[ExportMany]
	public class OrderNotifyGoodsShippedHandler : IOrderHandler {
		/// <summary>
		/// 在订单变为已发货时后台通知支付平台已发货
		/// </summary>
		public void OnStateChanged(SellerOrder order, OrderState state) {
			if (state == OrderState.WaitingBuyerConfirm) {
				var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
				var lastDelivery = order.OrderDeliveries
					.OrderByDescending(d => d.CreateTime).FirstOrDefault();
				var logisticsName = lastDelivery?.Logistics?.Name;
				var invoiceNo = lastDelivery?.LogisticsSerial;
				var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
				foreach (var transaction in orderManager.GetReleatedTransactions(order.Id)) {
					transactionManager.NotifyAllGoodsShippedBackground(
						transaction.Id, logisticsName, invoiceNo);
				}
			}
		}
	}
}
