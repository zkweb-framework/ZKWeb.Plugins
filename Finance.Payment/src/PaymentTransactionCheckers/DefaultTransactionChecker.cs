using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Model;

namespace ZKWeb.Plugins.Finance.Payment.src.PaymentTransactionCheckers {
	/// <summary>
	/// 默认的交易检查器
	/// </summary>
	[ExportMany]
	public class DefaultTransactionChecker : IPaymentTransactionChecker {
		/// <summary>
		/// 判断当前登录用户是否付款人的判断
		/// </summary>
		public void IsPayerLoggedIn(PaymentTransaction transaction, ref Tuple<bool, string> result) {
			// 条件：付款人是空或付款人和当前登录用户一致
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var userId = sessionManager.GetSession().ReleatedId;
			if (transaction.Payer == null || transaction.Payer.Id == userId) {
				result = Tuple.Create(true, (string)null);
			} else {
				result = Tuple.Create(false, (string)new T("Payer of transaction not logged in"));
			}
		}

		/// <summary>
		/// 判断交易是否可以付款
		/// </summary>
		public void IsPayable(PaymentTransaction transaction, ref Tuple<bool, string> result) {
			// 条件：状态是初始状态或等待支付
			if (transaction.State == PaymentTransactionState.Initial ||
				transaction.State == PaymentTransactionState.WaitingPaying) {
				result = Tuple.Create(true, (string)null);
			} else {
				result = Tuple.Create(false, (string)new T("Transaction not waiting for pay"));
			}
		}

		/// <summary>
		/// 判断交易是否可以处理等待付款
		/// 交易已经是相同状态时应该跳过处理而不是调用这个函数
		/// </summary>
		public void CanProcessWaitingPaying(PaymentTransaction transaction, ref Tuple<bool, string> result) {
			// 条件：状态是初始状态
			if (transaction.State == PaymentTransactionState.Initial) {
				result = Tuple.Create(true, (string)null);
			} else {
				result = Tuple.Create(false, (string)new T("Transaction not at initial state, can't set to waiting paying"));
			}
		}

		/// <summary>
		/// 判断交易是否可以处理担保交易已付款
		/// 交易已经是相同状态时应该跳过处理而不是调用这个函数
		/// </summary>
		public void CanProcessSecuredPaid(PaymentTransaction transaction, ref Tuple<bool, string> result) {
			// 条件：状态是初始状态或等待付款
			if (transaction.State == PaymentTransactionState.Initial ||
				transaction.State == PaymentTransactionState.WaitingPaying) {
				result = Tuple.Create(true, (string)null);
			} else {
				result = Tuple.Create(false, (string)new T("Transaction not waiting for pay"));
			}
		}

		/// <summary>
		/// 判断交易是否可以处理交易成功
		/// 交易已经是相同状态时应该跳过处理而不是调用这个函数
		/// </summary>
		public void CanProcessSuccess(PaymentTransaction transaction, ref Tuple<bool, string> result) {
			// 条件：状态是初始状态，等待付款或担保交易已付款
			if (transaction.State == PaymentTransactionState.Initial ||
				transaction.State == PaymentTransactionState.WaitingPaying ||
				transaction.State == PaymentTransactionState.SecuredPaid) {
				result = Tuple.Create(true, (string)null);
			} else {
				result = Tuple.Create(false, (string)new T("Transaction not waiting for pay or confirm, can't set to success"));
			}
		}

		/// <summary>
		/// 判断交易是否可以处理交易中止
		/// 交易已经是相同状态时应该跳过处理而不是调用这个函数
		/// </summary>
		public void CanProcessAborted(PaymentTransaction transaction, ref Tuple<bool, string> result) {
			// 条件：状态不是交易终止
			if (transaction.State != PaymentTransactionState.Aborted) {
				result = Tuple.Create(true, (string)null);
			} else {
				result = Tuple.Create(false, (string)new T("Transaction already aborted, can't process again"));
			}
		}

		/// <summary>
		/// 判断交易是否可以调用发货接口
		/// </summary>
		public void CanSendGoods(PaymentTransaction transaction, ref Tuple<bool, string> result) {
			// 条件：状态是担保交易已付款
			if (transaction.State == PaymentTransactionState.SecuredPaid) {
				result = Tuple.Create(true, (string)null);
			} else {
				result = Tuple.Create(false, (string)new T("Only secured paid transaction can call send goods api"));
			}
		}
	}
}
