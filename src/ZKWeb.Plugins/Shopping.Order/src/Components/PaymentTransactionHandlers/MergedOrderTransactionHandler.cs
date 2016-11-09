using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentTransactionHandlers.Bases;
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
	}
}
