using System.Collections.Generic;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Structs {
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
		public ProductMatchParameters MatchParameters { get; set; }
		/// <summary>
		/// 附加数据
		/// 计算价格等时可以使用这里传递数据，但创建订单后不会保留
		/// </summary>
		public IDictionary<string, object> Extra { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public CreateOrderProductParameters() {
			MatchParameters = new ProductMatchParameters();
			Extra = new Dictionary<string, object>();
		}
	}
}
