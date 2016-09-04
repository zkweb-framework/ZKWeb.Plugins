using ZKWeb.Plugins.Finance.Payment.src.PaymentTransactionHandlers;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.PaymentTransactionHandlers {
	/// <summary>
	/// 订单合并交易处理器
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
