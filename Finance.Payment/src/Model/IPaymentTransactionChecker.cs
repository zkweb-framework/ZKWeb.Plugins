using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Finance.Payment.src.Database;

namespace ZKWeb.Plugins.Finance.Payment.src.Model {
	/// <summary>
	/// 交易检查器的接口
	/// 可以修改检查结果，也可以不修改
	/// </summary>
	public interface IPaymentTransactionChecker {
		/// <summary>
		/// 判断当前登录用户是否付款人的判断
		/// </summary>
		/// <param name="transaction">交易</param>
		/// <param name="result">判断结果</param>
		void IsPayerLoggedIn(PaymentTransaction transaction, ref Tuple<bool, string> result);

		/// <summary>
		/// 判断交易是否可以付款
		/// </summary>
		/// <param name="transaction">交易</param>
		/// <param name="result">判断结果</param>
		void IsPayable(PaymentTransaction transaction, ref Tuple<bool, string> result);

		/// <summary>
		/// 判断交易是否可以处理等待付款
		/// </summary>
		/// <param name="transaction">交易</param>
		/// <param name="result">判断结果</param>
		void CanProcessWaitingPaying(PaymentTransaction transaction, ref Tuple<bool, string> result);

		/// <summary>
		/// 判断交易是否可以处理担保交易已付款
		/// </summary>
		/// <param name="transaction">交易</param>
		/// <param name="result">判断结果</param>
		void CanProcessSecuredPaid(PaymentTransaction transaction, ref Tuple<bool, string> result);

		/// <summary>
		/// 判断交易是否可以处理交易成功
		/// </summary>
		/// <param name="transaction">交易</param>
		/// <param name="result">判断结果</param>
		void CanProcessSuccess(PaymentTransaction transaction, ref Tuple<bool, string> result);

		/// <summary>
		/// 判断交易是否可以处理交易中止
		/// </summary>
		/// <param name="transaction">交易</param>
		/// <param name="result">判断结果</param>
		void CanProcessAborted(PaymentTransaction transaction, ref Tuple<bool, string> result);

		/// <summary>
		/// 判断交易是否可以进行发货
		/// </summary>
		/// <param name="transaction">交易</param>
		/// <param name="result">判断结果</param>
		void CanSendGoods(PaymentTransaction transaction, ref Tuple<bool, string> result);
	}
}
