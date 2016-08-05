using System.Collections.Generic;

namespace ZKWeb.Plugins.Shopping.Order.src.Model {
	/// <summary>
	/// 创建订单的参数
	/// 可用于计算订单价格和创建订单
	/// 可以从Json反序列化（除了下单的用户Id外）
	/// </summary>
	public class CreateOrderParameters {
		/// <summary>
		/// 下单的用户Id
		/// 未登录时等于null
		/// </summary>
		public long? UserId { get; set; }
		/// <summary>
		/// 下单的会话Id
		/// </summary>
		public string SessionId { get; set; }
		/// <summary>
		/// 订单参数
		/// 包含收货地址，选择的物流Id和收款接口Id等
		/// 格式 {
		///		ShippingAddress: { Country: ..., RegionId: ..., ... },
		///		SellerToLogistics: { SellerId: LogisticsId, ... },
		///		PaymentApiId: ...,
		///		CartProducts: { Id: Count, ... },
		///		...
		/// }
		/// </summary>
		public IDictionary<string, object> OrderParameters { get; set; }
		/// <summary>
		/// 创建订单商品的参数列表
		/// </summary>
		public IList<CreateOrderProductParameters> OrderProductParametersList { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public CreateOrderParameters() {
			OrderParameters = new Dictionary<string, object>();
			OrderProductParametersList = new List<CreateOrderProductParameters>();
		}
	}
}
