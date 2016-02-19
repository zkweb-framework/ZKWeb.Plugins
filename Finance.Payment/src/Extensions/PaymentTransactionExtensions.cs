using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Finance.Payment.src.Extensions {
	/// <summary>
	/// 支付交易的扩展函数
	/// </summary>
	public static class WebReceiveFundsTransactionExtensions {
		/// <summary>
		/// 检查函数的类型
		/// </summary>
		/// <param name="transaction">交易</param>
		/// <param name="result">判断结果</param>
		public delegate void CheckFunc(PaymentTransaction transaction, ref Tuple<bool, string> result);

		/// <summary>
		/// 检查交易是否满足指定条件
		/// 返回是否满足和文本信息
		/// </summary>
		/// <param name="transaction">交易</param>
		/// <returns></returns>
		public static Tuple<bool, string> Check(
			this PaymentTransaction transaction, Func<IPaymentTransactionChecker, CheckFunc> getCheckFunc) {
			var result = Tuple.Create(false, "No Result");
			var checkers = Application.Ioc.ResolveMany<IPaymentTransactionChecker>();
			checkers.ForEach(c => getCheckFunc(c)(transaction, ref result));
			return result;
		}
	}
}
