using System.Collections.Generic;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Structs {
	/// <summary>
	/// 创建订单的结果
	/// </summary>
	public class CreateOrderResult {
		/// <summary>
		/// 创建的买家订单列表
		/// </summary>
		public IList<BuyerOrder> CreatedOrders { get; set; }
		/// <summary>
		/// 创建的支付交易列表
		/// 如果有多个，最后一个支付交易应该是合并交易
		/// </summary>
		public IList<PaymentTransaction> CreatedTransactions { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public CreateOrderResult() {
			CreatedOrders = new List<BuyerOrder>();
			CreatedTransactions = new List<PaymentTransaction>();
		}
	}
}
