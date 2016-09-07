using System;
using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentTransactionHandlers;
using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentTransactionHandlers.Interfaces;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Enums;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.PaymentTransactionHandlers {
	/// <summary>
	/// 订单交易处理器
	/// </summary>
	[ExportMany]
	public class OrderTransactionHandler : IPaymentTransactionHandler {
		/// <summary>
		/// 交易类型
		/// </summary>
		public string Type { get { return ConstType; } }
		public const string ConstType = "OrderTransaction";

		/// <summary>
		/// 交易创建后
		/// </summary>
		public void OnCreated(PaymentTransaction transaction) {
			// 记录到日志
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogTransaction(string.Format("OrderTransaction created: {0}", transaction.Serial));
		}

		/// <summary>
		/// 等待付款时
		/// </summary>
		public void OnWaitingPaying(
			PaymentTransaction transaction, PaymentTransactionState previousState) {
			// 记录到日志
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogTransaction(string.Format("OrderTransaction waiting paying: {0}", transaction.Serial));
		}

		/// <summary>
		/// 担保交易付款后
		/// </summary>
		public void OnSecuredPaid(PaymentTransaction transaction,
			PaymentTransactionState previousState, IList<AutoSendGoodsParameters> parameters) {
			// TODO:记录到日志

			// TODO: 记录到订单记录

			// TODO: 处理订单已付款

		}

		/// <summary>
		/// 交易成功时
		/// </summary>
		public void OnSuccess(
			PaymentTransaction transaction, PaymentTransactionState previousState) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 交易终止时
		/// </summary>
		public void OnAbort(
			PaymentTransaction transaction, PaymentTransactionState previousState) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获取显示交易结果的Html
		/// </summary>
		public void GetResultHtml(PaymentTransaction transaction, IList<HtmlString> html) {
			// 显示内容
			// 根据订单状态显示
			// - WaitingBuyerPay: 订单创建成功，请付款
			// - WaitingSellerSendGoods: 您已付款成功，请等待卖家发货
			// - WaitingBuyerConfirm: 卖家已发货，请在收到货物后确认收货
			// - Success: 订单交易成功，感谢您的惠顾
			// - Cancelled: 订单已取消
			// - Invalid: 订单已作废
			// 订单编号: {编号} 订单金额: {金额} 账单金额: {金额}
			// [[使用{接口}支付]]

			// 获取关联订单
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var sellerOrderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var sellerOrder = sellerOrderManager.Get(transaction.ReleatedId ?? Guid.Empty);
			if (sellerOrder == null) {
				html.Add(new HtmlString(templateManager.RenderTemplate(
					"shopping.order/order_checkout.html", null)));
				return;
			}
			// 可支付时，显示支付按钮跳转到支付页面
			// 不可支付时，跳转到订单详情页（按流水号而不是买家订单Id）
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			html.Add(new HtmlString(templateManager.RenderTemplate(
				"shopping.order/order_checkout.html", new {
					orderSerial = sellerOrder.Serial,
					orderState = sellerOrder.State.ToString(),
					orderAmount = currencyManager
						.GetCurrency(sellerOrder.Currency).Format(sellerOrder.TotalCost),
					transactionAmount = currencyManager
						.GetCurrency(transaction.CurrencyType).Format(transaction.Amount),
					isPayable = transaction.Check(x => x.IsPayable).First,
					apiName = new T(transaction.Api.Name),
					paymentUrl = transactionManager.GetPaymentUrl(transaction.Id)
				})));
		}
	}
}
