using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Model;
using ZKWebStandard.Collection;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.PaymentTransactionHandlers {
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
		public void OnCreated(DatabaseContext context, PaymentTransaction transaction) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 等待付款时
		/// </summary>
		public void OnWaitingPaying(
			DatabaseContext context, PaymentTransaction transaction, PaymentTransactionState previousState) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 担保交易付款后
		/// </summary>
		public void OnSecuredPaid(
			DatabaseContext context, PaymentTransaction transaction,
			PaymentTransactionState previousState, IList<AutoSendGoodsParameters> parameters) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 交易成功时
		/// </summary>
		public void OnSuccess(
			DatabaseContext context, PaymentTransaction transaction, PaymentTransactionState previousState) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 交易终止时
		/// </summary>
		public void OnAbort(
			DatabaseContext context, PaymentTransaction transaction, PaymentTransactionState previousState) {
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
