using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ZKWeb.Localize;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.GenericRecord.src.Domain.Entities;
using ZKWeb.Plugins.Common.GenericRecord.src.Domain.Services;
using ZKWeb.Plugins.Common.SerialGenerate.src.Components.SerialGenerate;
using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentTransactionHandlers;
using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentTransactionHandlers.Bases;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Enums;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Structs;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.src.Domain.Services {
	/// <summary>
	/// 支付交易管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class PaymentTransactionManager : DomainServiceBase<PaymentTransaction, Guid> {
		/// <summary>
		/// 创建交易
		/// </summary>
		/// <param name="transactionType">交易类型</param>
		/// <param name="paymentApiId">支付接口Id</param>
		/// <param name="amount">交易金额</param>
		/// <param name="paymentFee">手续费</param>
		/// <param name="currencyType">货币类型</param>
		/// <param name="payerId">付款人Id</param>
		/// <param name="payeeId">收款人Id</param>
		/// <param name="releatedId">关联对象Id</param>
		/// <param name="description">描述</param>
		/// <param name="extraData">附加数据</param>
		/// <param name="releatedTransactions">关联的交易列表</param>
		/// <returns></returns>
		public virtual PaymentTransaction CreateTransaction(
			string transactionType, Guid paymentApiId,
			decimal amount, decimal paymentFee, string currencyType, Guid? payerId, Guid? payeeId,
			Guid? releatedId, string description, object extraData = null,
			IList<PaymentTransaction> releatedTransactions = null) {
			using (UnitOfWork.Scope()) {
				// 开启事务
				UnitOfWork.Context.BeginTransaction();
				// 检查参数
				if (amount <= 0) {
					throw new BadRequestException("Transaction amount must large than zero");
				} else if (string.IsNullOrEmpty(description)) {
					throw new BadRequestException("Transaction description is required");
				}
				// 检查接口是否可以使用
				var apiManager = Application.Ioc.Resolve<PaymentApiManager>();
				var api = apiManager.Get(paymentApiId);
				if (api == null) {
					throw new BadRequestException(new T("Payment api not exist"));
				} else if (!api.SupportTransactionTypes.Contains(transactionType)) {
					throw new BadRequestException(new T("Selected payment api not support this transaction"));
				} else if (api.Deleted) {
					throw new BadRequestException(new T("Selected payment api is deleted"));
				}
				// 保存交易到数据库
				var userManager = Application.Ioc.Resolve<UserManager>();
				var transaction = new PaymentTransaction() {
					Type = transactionType,
					Api = api,
					Amount = amount,
					PaymentFee = paymentFee,
					CurrencyType = currencyType,
					Payer = (payerId == null) ? null : userManager.Get(payerId.Value),
					Payee = (payeeId == null) ? null : userManager.Get(payeeId.Value),
					ReleatedId = releatedId,
					Description = description,
					State = PaymentTransactionState.Initial,
					ExtraData = extraData.ConvertOrDefault<PaymentTransactionExtraData>()
				};
				if (releatedTransactions != null) {
					transaction.ReleatedTransactions.AddRange(releatedTransactions);
				}
				transaction.Serial = SerialGenerator.GenerateFor(transaction);
				Save(ref transaction);
				// 调用创建交易后的处理
				var handlers = transaction.GetHandlers();
				handlers.ForEach(h => h.OnCreated(transaction));
				// 记录结果到数据库
				AddDetailRecord(transaction.Id, null, new T("Transaction Created"));
				// 结束事务
				UnitOfWork.Context.FinishTransaction();
				return transaction;
			}
		}

		/// <summary>
		/// 交易明细记录类型
		/// </summary>
		public const string RecordType = "PaymentTransactionDetail";

		/// <summary>
		/// 添加交易明细记录
		/// </summary>
		/// <param name="transactionId">交易Id</param>
		/// <param name="creatorId">创建人Id</param>
		/// <param name="content">内容</param>
		/// <param name="extraData">附加数据</param>
		public virtual void AddDetailRecord(
			Guid transactionId, Guid? creatorId, string content, object extraData = null) {
			var recordManager = Application.Ioc.Resolve<GenericRecordManager>();
			recordManager.AddRecord(
				RecordType, transactionId, creatorId, content, null, extraData);
		}

		/// <summary>
		/// 获取指定交易的所有明细记录
		/// </summary>
		/// <param name="transactionId">交易Id</param>
		/// <returns></returns>
		public virtual IList<GenericRecord> GetDetailRecords(Guid transactionId) {
			var recordManager = Application.Ioc.Resolve<GenericRecordManager>();
			return recordManager.FindRecords(RecordType, transactionId);
		}

		/// <summary>
		/// 根据交易类型和关联Id获取交易列表
		/// </summary>
		/// <param name="type">交易类型</param>
		/// <param name="releatedId">关联Id</param>
		/// <returns></returns>
		public virtual IList<PaymentTransaction> GetMany(string type, Guid? releatedId) {
			return GetMany(t => t.Type == type && t.ReleatedId == releatedId);
		}

		/// <summary>
		/// 设置交易最后发生的错误
		/// </summary>
		/// <param name="transactionId">交易Id</param>
		/// <param name="lastError">最后发生的错误</param>
		public virtual void SetLastError(Guid transactionId, string lastError) {
			using (UnitOfWork.Scope()) {
				// 开始事务
				UnitOfWork.Context.BeginTransaction();
				// 更新交易
				var transaction = Get(transactionId);
				if (transaction == null) {
					throw new BadRequestException(new T("Payment transaction not found"));
				}
				Save(ref transaction, t => t.LastError = lastError);
				// 记录错误到日志
				var logManager = Application.Ioc.Resolve<LogManager>();
				var message =
					new T("Payment transaction {0} error: {1}", transaction.Serial, lastError);
				logManager.LogTransaction(message);
				// 记录错误到数据库
				AddDetailRecord(transactionId, null, message, null);
				// 结束事务
				UnitOfWork.Context.FinishTransaction();
			}
		}

		/// <summary>
		/// 尝试把交易切换到指定的交易状态
		/// </summary>
		/// <param name="transactionId">交易Id</param>
		/// <param name="externalSerial">外部交易流水号，等于空时不更新</param>
		/// <param name="state">交易状态</param>
		protected virtual void ProcessInternal(
			Guid transactionId, string externalSerial, PaymentTransactionState state) {
			using (UnitOfWork.Scope()) {
				// 开启事务
				UnitOfWork.Context.BeginTransaction();
				// 获取交易
				var transaction = Get(transactionId);
				if (transaction == null) {
					throw new BadRequestException(new T("Payment transaction not found"));
				}
				// 判断是否可以处理指定的交易状态
				// 已经是指定的交易状态时返回成功
				Pair<bool, string> result;
				if (transaction.State == state) {
					return;
				} else if (state == PaymentTransactionState.WaitingPaying) {
					result = transaction.Check(c => c.CanProcessWaitingPaying);
				} else if (state == PaymentTransactionState.SecuredPaid) {
					result = transaction.Check(c => c.CanProcessSecuredPaid);
				} else if (state == PaymentTransactionState.Success) {
					result = transaction.Check(c => c.CanProcessSuccess);
				} else if (state == PaymentTransactionState.Aborted) {
					result = transaction.Check(c => c.CanProcessAborted);
				} else {
					throw new BadRequestException(new T("Unsupported transaction state: {0}", state));
				}
				if (!result.First) {
					throw new BadRequestException(result.Second);
				}
				// 获取交易类型对应的处理器
				var handlers = transaction.GetHandlers();
				// 设置交易状态
				var previousState = transaction.State;
				Save(ref transaction, t => {
					t.State = state;
					t.LastError = null; // 清空最后发生的错误
					if (!string.IsNullOrEmpty(externalSerial)) {
						t.ExternalSerial = externalSerial; // 更新外部流水号
					}
				});
				// 使用处理器处理指定的交易状态
				var parameters = new List<AutoDeliveryGoodsParameters>();
				if (state == PaymentTransactionState.WaitingPaying) {
					handlers.ForEach(h => h.OnWaitingPaying(transaction, previousState));
				} else if (state == PaymentTransactionState.SecuredPaid) {
					handlers.ForEach(h => h.OnSecuredPaid(transaction, previousState, parameters));
				} else if (state == PaymentTransactionState.Success) {
					handlers.ForEach(h => h.OnSuccess(transaction, previousState));
				} else if (state == PaymentTransactionState.Aborted) {
					handlers.ForEach(h => h.OnAbort(transaction, previousState));
				} else {
					throw new BadRequestException(new T("Unsupported transaction state: {0}", state));
				}
				// 成功时添加详细记录
				AddDetailRecord(transaction.Id, null,
					new T("Change transaction state to {0}", new T(transaction.State.GetDescription())));
				// 需要自动发货时进行发货
				foreach (var parameter in parameters) {
					NotifyAllGoodsShippedBackground(
						transaction.Id, parameter.LogisticsName, parameter.InvoiceNo);
				}
				// 结束事务
				UnitOfWork.Context.FinishTransaction();
			}
		}

		/// <summary>
		/// 尝试把交易切换到指定的交易状态
		/// 失败时记录错误并抛出例外
		/// </summary>
		/// <param name="transactionId">交易Id</param>
		/// <param name="externalSerial">外部交易流水号，等于空时不更新</param>
		/// <param name="state">交易状态</param>
		public virtual void Process(
			Guid transactionId, string externalSerial, PaymentTransactionState state) {
			try {
				ProcessInternal(transactionId, externalSerial, state);
			} catch (Exception e) {
				var errorMessage =
					new T("Change transaction state to {0} failed: {1}",
					new T(state.GetDescription()),
					new T(e.Message));
				AddDetailRecord(transactionId, null, errorMessage);
				SetLastError(transactionId, errorMessage);
				throw;
			}
		}

		/// <summary>
		/// 通知支付平台已发货
		/// 仅在状态是担保交易已付款时通知，否则不做处理
		/// 通知会在后台进行，出错时会记录到日志
		/// </summary>
		/// <param name="transactionId">交易Id</param>
		/// <param name="logisticsName">快递或物流名称</param>
		/// <param name="invoiceNo">快递单号</param>
		public virtual void NotifyAllGoodsShippedBackground(
			Guid transactionId, string logisticsName, string invoiceNo) {
			// 获取交易
			var transaction = Get(transactionId);
			if (transaction == null) {
				throw new BadRequestException(new T("Payment transaction not found"));
			}
			// 判断是否需要通知发货
			var result = transaction.Check(c => c.CanDeliveryGoods);
			if (!result.First) {
				return;
			}
			// 后台调用支付接口的处理器处理发货，并记录到记录
			var handlers = transaction.Api.GetHandlers();
			ThreadPool.QueueUserWorkItem(_ => {
				try {
					try {
						handlers.ForEach(h => h.DeliveryGoods(transaction, logisticsName, invoiceNo));
						AddDetailRecord(transactionId, null,
							new T("Notify goods shipped to payment api success"));
					} catch (Exception e) {
						AddDetailRecord(transactionId, null,
							new T("Notify goods shipped to payment api failed: {0}", e.Message));
					}
				} catch {
					// 进程池中的任务不能抛出例外
				}
			});
		}

		/// <summary>
		/// 确保终止关联了指定交易的交易
		/// 一般用于子交易有变化时终止合并交易
		/// 如果合并交易不存在或已终止，这个函数什么都不做
		/// </summary>
		/// <param name="transactionIds">子交易的Id列表</param>
		/// <param name="operatorId">操作人Id</param>
		/// <param name="reason">作废原因</param>
		public virtual void EnsureParentTransactionAborted(
			IList<Guid> transactionIds, Guid? operatorId, string reason) {
			var parentTransactionIds = Repository.Query()
				.Where(t => t.ReleatedTransactions.Any(rt => transactionIds.Contains(rt.Id)))
				.Where(t => t.State != PaymentTransactionState.Aborted)
				.Select(t => t.Id).ToList();
			foreach (var id in parentTransactionIds) {
				AddDetailRecord(id, operatorId, reason);
				Process(id, null, PaymentTransactionState.Aborted);
			}
		}

		/// <summary>
		/// 如果处理交易的来源不是合并交易，则确保合并交易中止
		/// 一般用于子交易单独支付后自动终止合并交易
		/// 如果处理交易的来源是合并交易，或合并交易不存在或已终止，这个函数什么都不做
		/// </summary>
		/// <param name="transaction">子交易</param>
		/// <param name="operatorId">操作人Id</param>
		/// <param name="reason">作废原因</param>
		public virtual void EnsureParentTransactionAbortedIfProcessNotFromParent(
			PaymentTransaction transaction, Guid? operatorId, string reason) {
			if (transaction.ExternalSerial == null ||
				!transaction.ExternalSerial.StartsWith(MergedTransactionHandlerBase.ExternalSerialPrefix)) {
				EnsureParentTransactionAborted(new[] { transaction.Id }, operatorId, reason);
			}
		}

		/// <summary>
		/// 创建合并交易，合并金额和重新计算手续费
		/// 要求所有交易可支付，并且所有交易的货币，支付接口，收款人，付款人等一致
		/// </summary>
		/// <param name="transactionType">交易类型</param>
		/// <param name="childTransactions">子交易列表</param>
		/// <returns></returns>
		public virtual PaymentTransaction CreateMergedTransaction(
			string transactionType,
			IList<PaymentTransaction> childTransactions) {
			// 检查各个子交易
			var firstTransaction = childTransactions.First();
			if (childTransactions.Any(t => t.CurrencyType != firstTransaction.CurrencyType)) {
				throw new BadRequestException(new T("Some child transaction have different currency type"));
			} else if (childTransactions.Any(t => t.Api?.Id != firstTransaction.Api?.Id)) {
				throw new BadRequestException(new T("Some child transaction have different payment api"));
			} else if (childTransactions.Any(t => t.Payer?.Id != firstTransaction.Payer?.Id)) {
				throw new BadRequestException(new T("Some child transaction have different payer"));
			} else if (childTransactions.Any(t => t.Payee?.Id != firstTransaction.Payee?.Id)) {
				throw new BadRequestException(new T("Some child transaction have different payee"));
			} else if (childTransactions.Any(t => !t.Check(c => c.IsPayable).First)) {
				throw new BadRequestException(new T("Some child transaction is not payable"));
			}
			// 作废原有的合并交易
			EnsureParentTransactionAborted(
				childTransactions.Select(t => t.Id).ToList(), null,
				new T("New merged transaction has created, this merge transaction should be aborted"));
			// 创建新的合并交易
			var apiManager = Application.Ioc.Resolve<PaymentApiManager>();
			var amount = childTransactions.Sum(t => t.Amount);
			var paymentFee = apiManager.CalculatePaymentFee(firstTransaction.Api.Id, amount);
			var transaction = CreateTransaction(
				transactionType,
				firstTransaction.Api.Id,
				amount,
				paymentFee,
				firstTransaction.CurrencyType,
				firstTransaction.Payer?.Id,
				firstTransaction.Payee?.Id,
				null, // 无关联Id
				string.Join("; ", childTransactions.Select(t => t.Description)),
				null, // 无附加数据
				childTransactions);
			return transaction;
		}

		/// <summary>
		/// 获取一个指定用户可见的交易Id
		/// 返回的Id不固定，也可能返回Guid.Empty
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <returns></returns>
		public virtual Guid SelectOneVisibleTransactionId(Guid userId) {
			using (UnitOfWork.Scope()) {
				return Repository.Query()
					.Where(t => t.Payer != null && t.Payer.Id == userId &&
						(t.State == PaymentTransactionState.Initial ||
						t.State == PaymentTransactionState.WaitingPaying))
					.Select(t => t.Id)
					.FirstOrDefault();
			}
		}

		/// <summary>
		/// 获取支付Url，创建交易后可以跳转到这个Url进行支付
		/// </summary>
		public virtual string GetPaymentUrl(Guid transactionId) {
			return string.Format("/payment/transaction/pay?id={0}", transactionId);
		}

		/// <summary>
		/// 获取查看结果的Url，支付成功或失败后可以跳转到这个Url显示结果
		/// </summary>
		public virtual string GetResultUrl(Guid transactionId) {
			return string.Format("/payment/transaction/pay_result?id={0}", transactionId);
		}
	}
}
