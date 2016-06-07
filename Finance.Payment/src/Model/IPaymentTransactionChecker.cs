using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Utils.Collections;

namespace ZKWeb.Plugins.Finance.Payment.src.Model {
	/// <summary>
	/// 交易检查器的接口
	/// 可以修改检查结果，也可以不修改
	/// </summary>
	public interface IPaymentTransactionChecker {
		/// <summary>
		/// 判断交易是否可以付款
		/// </summary>
		/// <param name="transaction">交易</param>
		/// <param name="result">判断结果</param>
		void IsPayable(PaymentTransaction transaction, ref Pair<bool, string> result);

		/// <summary>
		/// 判断付款人是否已登陆
		/// 这个函数用于判断当前用户是否有权限查看和支付指定的交易
		/// </summary>
		/// <param name="transaction">交易</param>
		/// <param name="result">判断结果</param>
		void IsPayerLoggedIn(PaymentTransaction transaction, ref Pair<bool, string> result);

		/// <summary>
		/// 判断交易是否可以处理等待付款
		/// 交易已经是相同状态时应该跳过处理而不是调用这个函数
		/// </summary>
		/// <param name="transaction">交易</param>
		/// <param name="result">判断结果</param>
		void CanProcessWaitingPaying(PaymentTransaction transaction, ref Pair<bool, string> result);

		/// <summary>
		/// 判断交易是否可以处理担保交易已付款
		/// 交易已经是相同状态时应该跳过处理而不是调用这个函数
		/// </summary>
		/// <param name="transaction">交易</param>
		/// <param name="result">判断结果</param>
		void CanProcessSecuredPaid(PaymentTransaction transaction, ref Pair<bool, string> result);

		/// <summary>
		/// 判断交易是否可以处理交易成功
		/// 交易已经是相同状态时应该跳过处理而不是调用这个函数
		/// </summary>
		/// <param name="transaction">交易</param>
		/// <param name="result">判断结果</param>
		void CanProcessSuccess(PaymentTransaction transaction, ref Pair<bool, string> result);

		/// <summary>
		/// 判断交易是否可以处理交易中止
		/// 交易已经是相同状态时应该跳过处理而不是调用这个函数
		/// </summary>
		/// <param name="transaction">交易</param>
		/// <param name="result">判断结果</param>
		void CanProcessAborted(PaymentTransaction transaction, ref Pair<bool, string> result);

		/// <summary>
		/// 判断交易是否可以调用发货接口
		/// </summary>
		/// <param name="transaction">交易</param>
		/// <param name="result">判断结果</param>
		void CanSendGoods(PaymentTransaction transaction, ref Pair<bool, string> result);
	}
}
