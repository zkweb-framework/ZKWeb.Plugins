using System.Collections.Generic;

namespace ZKWeb.Plugins.Shopping.Order.src.Model {
	/// <summary>
	/// 创建订单商品的参数
	/// 可用于计算订单商品价格和创建订单
	/// 可以从Json反序列化
	/// </summary>
	public class CreateOrderProductParameters {
		/// <summary>
		/// 商品Id
		/// </summary>
		public long ProductId { get; set; }
		/// <summary>
		/// 商品匹配参数
		/// 订购数量在OrderCount键下
		/// </summary>
		public IDictionary<string, object> MatchParameters { get; set; }
		/// <summary>
		/// 附加数据
		/// </summary>
		public IDictionary<string, object> Extra { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public CreateOrderProductParameters() {
			MatchParameters = new Dictionary<string, object>();
			Extra = new Dictionary<string, object>();
		}
	}
}