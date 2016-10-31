using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Structs {
	/// <summary>
	/// 修改订单价格的参数
	/// 用于修改订单，订单商品和订单交易的金额
	/// 可以从Json反序列化
	/// </summary>
	public class OrderEditCostParameters {
		/// <summary>
		/// 订单商品数量的修改
		/// { 订单商品Id: 数量，删除时为0 }
		/// </summary>
		public IDictionary<Guid, long> OrderProductCountMapping { get; set; }
		/// <summary>
		/// 订单商品单价的修改
		/// </summary>
		public IDictionary<Guid, decimal> OrderProductUnitPriceMapping { get; set; }
		/// <summary>
		/// 订单总金额的修改
		/// [{ Delta: 100, Type: "ProductTotalPrice" }]
		/// </summary>
		public IList<OrderPriceCalcResult.Part> OrderTotalCostCalcResult { get; set; }
		/// <summary>
		/// 订单交易金额的修改
		/// { 交易Id: 金额 }
		/// </summary>
		public IDictionary<Guid, decimal> TransactionAmountMapping { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public OrderEditCostParameters() {
			OrderProductCountMapping = new Dictionary<Guid, long>();
			OrderProductUnitPriceMapping = new Dictionary<Guid, decimal>();
			OrderTotalCostCalcResult = new List<OrderPriceCalcResult.Part>();
			TransactionAmountMapping = new Dictionary<Guid, decimal>();
		}
	}
}
