using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentTransactionHandlers.Bases;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWebStandard.Collection;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.PaymentTransactionHandlers {
	/// <summary>
	/// 订单合并交易处理器
	/// 
	/// 重复支付 情况a
	/// - 创建订单A和订单B
	/// - 打开合并交易支付页面
	/// - 打开订单A交易支付页面
	/// - 支付合并交易
	/// - 支付订单A交易（这时会重复OnSuccess）
	///
	/// 重复支付 情况b
	/// - 创建订单A和订单B
	/// - 打开合并交易支付页面
	/// - 打开订单A交易支付页面
	/// - 支付订单A交易
	/// - 支付合并交易（这时会重复OnSuccess）
	/// 
	/// 对于重复支付合并交易和普通交易的处理
	/// - 修改价格时自动终止合并交易, 使用EnsureParentTransactionAborted
	/// - TODO: 普通交易支付完成时自动终止合并交易，适用于普通交易支付在先
	/// - TODO: 普通交易支付完成时如果查到合并交易同样支付完成则记录错误，适用于合并交易支付在先
	/// </summary>
	[ExportMany]
	public class MergedOrderTransactionHandler : MergedTransactionHandlerBase {
		/// <summary>
		/// 交易类型
		/// </summary>
		public override string Type { get { return ConstType; } }
		public const string ConstType = "MergedOrderTransaction";

		/// <summary>
		/// 获取显示交易结果的Html
		/// </summary>
		public override void GetResultHtml(PaymentTransaction transaction, IList<HtmlString> html) {
			// 获取所有关联订单

			// TODO

			// 如果交易本身不可支付，根据交易状态显示信息
			// 否则
			// - 如果所有订单可支付，则显示支付按钮跳转到支付页面
			// - 否则显示错误信息（例如部分订单已支付，部分未支付）

			// TODO
		}
	}
}
