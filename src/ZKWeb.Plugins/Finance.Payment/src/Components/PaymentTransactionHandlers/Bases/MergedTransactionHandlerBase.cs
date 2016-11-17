using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Logging;
using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentTransactionHandlers.Interfaces;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Enums;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Finance.Payment.src.Components.PaymentTransactionHandlers.Bases {
	/// <summary>
	/// 合并交易处理器的基类
	/// 因为合并交易需要安全检查和区分类别
	/// 这个处理器不注册到容器中，需要继承使用
	/// </summary>
	public abstract class MergedTransactionHandlerBase : IPaymentTransactionHandler {
		/// <summary>
		/// 设置关联交易的外部流水号时使用的前缀
		/// </summary>
		public const string ExternalSerialPrefix = "MERGED_";
		/// <summary>
		/// 交易类型
		/// </summary>
		public abstract string Type { get; }

		/// <summary>
		/// 交易创建后
		/// </summary>
		public virtual void OnCreated(PaymentTransaction transaction) {
			var logManager = Application.Ioc.Resolve<LogManager>();
			foreach (var childTransaction in transaction.ReleatedTransactions) {
				logManager.LogTransaction(
					$"Merged transaction {transaction.Serial} created " +
					$"with child transaction {childTransaction.Serial}");
				var handlers = childTransaction.GetHandlers();
				handlers.ForEach(h => h.OnCreated(childTransaction));
			}
		}

		/// <summary>
		/// 等待付款时
		/// </summary>
		public virtual void OnWaitingPaying(
			PaymentTransaction transaction, PaymentTransactionState previousState) {
			var logManager = Application.Ioc.Resolve<LogManager>();
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			foreach (var childTransaction in transaction.ReleatedTransactions) {
				var canProcessWaitingPaying = childTransaction.Check(c => c.CanProcessWaitingPaying);
				if (canProcessWaitingPaying.First) {
					// 处理关联交易的等待付款
					logManager.LogTransaction(
						$"Merged transaction {transaction.Serial} waiting paying " +
						$"with child transaction {childTransaction.Serial}");
					transactionManager.Process(
						childTransaction.Id,
						ExternalSerialPrefix + transaction.Serial,
						PaymentTransactionState.WaitingPaying);
				} else {
					// 有冲突时记录到交易记录
					logManager.LogTransaction(
						$"Merged transaction {transaction.Serial} waiting paying " +
						$"with child transaction {childTransaction.Serial} failed, " +
						$"reason is {canProcessWaitingPaying.Second}");
					transactionManager.AddDetailRecord(
						transaction.Id,
						null,
						string.Format(new T("Can't process waiting paying on child transaction {0}, reason is {1}"),
							childTransaction.Serial, canProcessWaitingPaying.Second));
				}
			}
		}

		/// <summary>
		/// 担保交易付款后
		/// </summary>
		public virtual void OnSecuredPaid(PaymentTransaction transaction,
			PaymentTransactionState previousState, IList<AutoDeliveryGoodsParameters> parameters) {
			var logManager = Application.Ioc.Resolve<LogManager>();
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			foreach (var childTransaction in transaction.ReleatedTransactions) {
				var canProcessSecuredPaid = childTransaction.Check(c => c.CanProcessSecuredPaid);
				if (canProcessSecuredPaid.First) {
					// 处理关联交易的担保交易已付款
					logManager.LogTransaction(
						$"Merged transaction {transaction.Serial} secured paid " +
						$"with child transaction {childTransaction.Serial}");
					transactionManager.Process(
						childTransaction.Id,
						ExternalSerialPrefix + transaction.Serial,
						PaymentTransactionState.SecuredPaid);
				} else {
					// 有冲突时记录到交易记录
					logManager.LogTransaction(
						$"Merged transaction {transaction.Serial} secured paid " +
						$"with child transaction {childTransaction.Serial} failed, " +
						$"reason is {canProcessSecuredPaid.Second}");
					transactionManager.AddDetailRecord(
						transaction.Id,
						null,
						string.Format(new T("Can't process secured paid on child transaction {0}, reason is {1}"),
							childTransaction.Serial, canProcessSecuredPaid.Second));
				}
			}
		}

		/// <summary>
		/// 交易成功时
		/// </summary>
		public virtual void OnSuccess(
			PaymentTransaction transaction, PaymentTransactionState previousState) {
			var logManager = Application.Ioc.Resolve<LogManager>();
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			foreach (var childTransaction in transaction.ReleatedTransactions) {
				var canProcessSuccess = childTransaction.Check(c => c.CanProcessSuccess);
				if (canProcessSuccess.First) {
					// 处理关联交易的交易成功
					logManager.LogTransaction(
						$"Merged transaction {transaction.Serial} success " +
						$"with child transaction {childTransaction.Serial}");
					transactionManager.Process(
						childTransaction.Id,
						ExternalSerialPrefix + transaction.Serial,
						PaymentTransactionState.Success);
				} else {
					// 有冲突时记录到交易记录
					logManager.LogTransaction(
						$"Merged transaction {transaction.Serial} success " +
						$"with child transaction {childTransaction.Serial} failed, " +
						$"reason is {canProcessSuccess.Second}");
					transactionManager.AddDetailRecord(
						transaction.Id,
						null,
						string.Format(new T("Can't process success on child transaction {0}, reason is {1}"),
							childTransaction.Serial, canProcessSuccess.Second));
				}
			}
		}

		/// <summary>
		/// 交易终止时
		/// 不会终止关联交易，只会清除所有关联交易
		/// </summary>
		public virtual void OnAbort(
			PaymentTransaction transaction, PaymentTransactionState previousState) {
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogTransaction(
				$"Merged transaction {transaction.Serial} aborted, clear all child transactions");
			transaction.ReleatedTransactions.Clear();
		}

		/// <summary>
		/// 获取显示交易结果的Html
		/// </summary>
		public abstract void GetResultHtml(PaymentTransaction transaction, IList<HtmlString> html);
	}
}
