using System.Collections.Generic;

namespace ZKWeb.Plugins.Shopping.Order.src.Model {
	/// <summary>
	/// 创建订单的结果
	/// </summary>
	public class CreateOrderResult {
		/// <summary>
		/// 创建的订单Id列表
		/// </summary>
		public IList<long> CreatedOrderIds { get; set; }
		/// <summary>
		/// 创建的支付交易Id列表
		/// 如果有多个，最后一个支付交易应该是合并交易
		/// </summary>
		public IList<long> CreatedTransactionIds { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public CreateOrderResult() {
			CreatedOrderIds = new List<long>();
			CreatedTransactionIds = new List<long>();
		}
	}
}
