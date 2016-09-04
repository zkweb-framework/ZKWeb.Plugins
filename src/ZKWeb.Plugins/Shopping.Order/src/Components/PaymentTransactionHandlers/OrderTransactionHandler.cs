using System;
using System.Collections.Generic;
using ZKWeb.Logging;
using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentTransactionHandlers;
using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentTransactionHandlers.Interfaces;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Enums;
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
			throw new NotImplementedException();
		}
	}
}
