using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Finance.Payment.src.Model {
	/// <summary>
	/// 支付交易的状态
	/// </summary>
	public enum PaymentTransactionState {
		/// <summary>
		/// 初始状态，这时可以修改金额和货币
		/// </summary>
		[LabelCssClass("label-default")]
		[Description("InitialState")]
		Initial = 0,
		/// <summary>
		/// 等待付款，这时金额和货币不能修改
		/// 部分接口不会使用这个状态
		/// </summary>
		[LabelCssClass("label-primary")]
		[Description("WaitingPaying")]
		WaitingPaying = 1,
		/// <summary>
		/// 担保交易已付款
		/// </summary>
		[LabelCssClass("label-warning")]
		[Description("SecuredPaid")]
		SecuredPaid = 2,
		/// <summary>
		/// 交易成功
		/// </summary>
		[LabelCssClass("label-success")]
		[Description("TransactionSuccess")]
		Success = 3,
		/// <summary>
		/// 交易中止
		/// </summary>
		[LabelCssClass("label-danger")]
		[Description("TransactionAborted")]
		Aborted = 4
	}
}
