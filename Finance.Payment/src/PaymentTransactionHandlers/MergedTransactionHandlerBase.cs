using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Model;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Finance.Payment.src.PaymentTransactionHandlers {
	/// <summary>
	/// 合并交易处理器的基类
	/// 因为合并交易需要安全检查和区分类别
	/// 这个处理器不注册到容器中，需要继承使用
	/// </summary>
	public abstract class MergedTransactionHandlerBase : IPaymentTransactionHandler {
		/// <summary>
		/// 交易类型
		/// </summary>
		public abstract string Type { get; }

		/// <summary>
		/// 交易创建后
		/// </summary>
		public virtual void OnCreated(IDatabaseContext context, PaymentTransaction transaction) {
			foreach (var childTransaction in transaction.ReleatedTransactions) {
				var handlers = childTransaction.GetHandlers();
				handlers.ForEach(h => h.OnCreated(context, childTransaction));
			}
		}

		/// <summary>
		/// 等待付款时
		/// </summary>
		public virtual void OnWaitingPaying(
			IDatabaseContext context, PaymentTransaction transaction, PaymentTransactionState previousState) {
			foreach (var childTransaction in transaction.ReleatedTransactions) {
				var handlers = childTransaction.GetHandlers();
				handlers.ForEach(h => h.OnWaitingPaying(context, childTransaction, previousState));
			}
		}

		/// <summary>
		/// 担保交易付款后
		/// </summary>
		public virtual void OnSecuredPaid(
			IDatabaseContext context, PaymentTransaction transaction,
			PaymentTransactionState previousState, IList<AutoSendGoodsParameters> parameters) {
			foreach (var childTransaction in transaction.ReleatedTransactions) {
				var handlers = childTransaction.GetHandlers();
				handlers.ForEach(h => h.OnSecuredPaid(context, childTransaction, previousState, parameters));
			}
		}

		/// <summary>
		/// 交易成功时
		/// </summary>
		public virtual void OnSuccess(
			IDatabaseContext context, PaymentTransaction transaction, PaymentTransactionState previousState) {
			foreach (var childTransaction in transaction.ReleatedTransactions) {
				var handlers = childTransaction.GetHandlers();
				handlers.ForEach(h => h.OnSuccess(context, childTransaction, previousState));
			}
		}

		/// <summary>
		/// 交易终止时
		/// 不会终止关联交易，只会清除所有关联交易
		/// </summary>
		public virtual void OnAbort(
			IDatabaseContext context, PaymentTransaction transaction, PaymentTransactionState previousState) {
			transaction.ReleatedTransactions.Clear();
		}

		/// <summary>
		/// 获取显示交易结果的Html
		/// </summary>
		public virtual void GetResultHtml(PaymentTransaction transaction, IList<HtmlString> html) {
			foreach (var childTransaction in transaction.ReleatedTransactions) {
				var handlers = childTransaction.GetHandlers();
				handlers.ForEach(h => h.GetResultHtml(childTransaction, html));
			}
		}
	}
}
