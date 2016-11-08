using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Enums;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderCheckers.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWebStandard.Collections;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderCheckers {
	/// <summary>
	/// 默认的订单检查器
	/// </summary>
	[ExportMany]
	public class DefaultOrderChecker : IOrderChecker {
		/// <summary>
		/// 判断订单是否可以付款
		/// 前台使用
		/// 允许条件: 订单状态是等待付款，且有可支付的关联交易
		/// </summary>
		public void CanPay(SellerOrder order, ref Pair<bool, string> result) {
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			if (order.State != OrderState.WaitingBuyerPay) {
				result = Pair.Create(false,
					new T("Order not payable because it's not waiting buyer pay").ToString());
			} else if (!orderManager.GetReleatedTransactions(order.Id).Any(t =>
				t.Check(c => c.IsPayable).First)) {
				result = Pair.Create(false,
					new T("Order not payable because no payable releated transactions").ToString());
			} else {
				result = Pair.Create(true, (string)null);
			}
		}

		/// <summary>
		/// 判断订单是否可以取消
		/// 前台使用
		/// 允许条件: 订单状态是等待付款，且关联的收款交易的状态是初始状态或等待付款
		/// </summary>
		public void CanSetCancelled(SellerOrder order, ref Pair<bool, string> result) {
			var orderManage = Application.Ioc.Resolve<SellerOrderManager>();
			if (order.State != OrderState.WaitingBuyerPay) {
				result = Pair.Create(false,
					new T("Order not cancellable because it's not waiting buyer pay").ToString());
			} else if (orderManage.GetReleatedTransactions(order.Id).Any(t =>
				t.State == PaymentTransactionState.SecuredPaid ||
				t.State == PaymentTransactionState.Success)) {
				result = Pair.Create(false,
					new T("Order not cancellable because some releated transaction already paid").ToString());
			} else {
				result = Pair.Create(true, (string)null);
			}
		}

		/// <summary>
		/// 判断订单是否可以确认收货
		/// 前台使用
		/// 允许条件: 订单状态是已发货
		/// </summary>
		public void CanConfirm(SellerOrder order, ref Pair<bool, string> result) {
			if (order.State != OrderState.WaitingBuyerConfirm) {
				result = Pair.Create(false,
					new T("Order can't be confirmed because it's not waiting buyer confirm").ToString());
			} else {
				result = Pair.Create(true, (string)null);
			}
		}

		/// <summary>
		/// 判断订单是否可以修改价格
		/// 后台使用
		/// 允许条件: 订单状态是等待付款，且关联的收款交易的状态是初始状态
		/// </summary>
		public void CanEditCost(SellerOrder order, ref Pair<bool, string> result) {
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			if (order.State != OrderState.WaitingBuyerPay) {
				result = Pair.Create(false,
					new T("Order can't edit cost because it's not waiting buyer pay").ToString());
			} else if (orderManager.GetReleatedTransactions(order.Id).Any(t =>
				t.State != PaymentTransactionState.Initial)) {
				result = Pair.Create(false,
					new T("Order can't edit cost because some releated transaction paid or realized").ToString());
			} else {
				result = Pair.Create(true, (string)null);
			}
		}

		/// <summary>
		/// 判断订单是否可以编辑收货地址
		/// 后台使用
		/// 允许条件: 订单状态是等待付款，或等待发货
		/// </summary>
		public void CanEditShippingAddress(SellerOrder order, ref Pair<bool, string> result) {
			if (order.State != OrderState.WaitingBuyerPay &&
				order.State != OrderState.WaitingSellerDeliveryGoods) {
				result = Pair.Create(false,
					new T("Order can't edit shipping address because it's not waiting buyer pay or waiting seller send goods").ToString());
			} else {
				result = Pair.Create(true, (string)null);
			}
		}

		/// <summary>
		/// 判断订单是否可以发货
		/// 后台使用
		/// 允许条件: 订单状态是等待发货
		/// </summary>
		public void CanDeliveryGoods(SellerOrder order, ref Pair<bool, string> result) {
			if (order.State != OrderState.WaitingSellerDeliveryGoods) {
				result = Pair.Create(false,
					new T("Order can't send goods because it's not waiting seller send goods").ToString());
			} else {
				result = Pair.Create(true, (string)null);
			}
		}

		/// <summary>
		/// 判断订单是否可以作废
		/// 后台使用
		/// 允许条件: 订单状态不是已作废或已取消
		/// </summary>
		public void CanSetInvalid(SellerOrder order, ref Pair<bool, string> result) {
			if (order.State == OrderState.OrderInvalid) {
				result = Pair.Create(false, new T("Order already invalid").ToString());
			} else if (order.State == OrderState.OrderCancelled) {
				result = Pair.Create(false, new T("Order already cancelled").ToString());
			} else {
				result = Pair.Create(true, (string)null);
			}
		}

		/// <summary>
		/// 是否可处理订单已付款
		/// 修改订单状态时使用
		/// 允许条件: 订单状态是等待付款，且关联的所有交易状态是交易成功或担保交易已付款
		/// </summary>
		public void CanProcessOrderPaid(SellerOrder order, ref Pair<bool, string> result) {
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			if (order.State != OrderState.WaitingBuyerPay) {
				result = Pair.Create(false,
					new T("Order can't be paid because it's not waiting buyer pay").ToString());
			} else if (orderManager.GetReleatedTransactions(order.Id)
				.Any(t => t.State != PaymentTransactionState.Success &&
					t.State != PaymentTransactionState.SecuredPaid)) {
				result = Pair.Create(false,
					new T("Order can't be paid because not all releated transaction paid").ToString());
			} else {
				result = Pair.Create(true, (string)null);
			}
		}

		/// <summary>
		/// 是否可处理订单所有商品已发货
		/// 修改订单状态时使用
		/// 允许条件: 订单状态是等待发货，且所有商品都已经发货
		/// </summary>
		public void CanProcessAllGoodsShipped(SellerOrder order, ref Pair<bool, string> result) {
			if (order.State != OrderState.WaitingSellerDeliveryGoods) {
				result = Pair.Create(false,
					new T("Order can't be shipped because not waiting seller send goods").ToString());
			} else {
				var allShipped = order.OrderProducts.All(p => p.Deliveries.Sum(d => d.Count) >= p.Count);
				if (!allShipped) {
					result = Pair.Create(false,
						new T("Order can't be shipped because not all goods shipped").ToString());
				} else {
					result = Pair.Create(true, (string)null);
				}
			}
		}

		/// <summary>
		/// 是否可处理交易成功
		/// 修改订单状态时使用
		/// 允许条件: 订单状态是已发货
		/// </summary>
		public void CanProcessSuccess(SellerOrder order, ref Pair<bool, string> result) {
			if (order.State != OrderState.WaitingBuyerConfirm) {
				result = Pair.Create(false,
					new T("Order can't be success because not waiting buyer confirm").ToString());
			} else {
				result = Pair.Create(true, (string)null);
			}
		}
	}
}
