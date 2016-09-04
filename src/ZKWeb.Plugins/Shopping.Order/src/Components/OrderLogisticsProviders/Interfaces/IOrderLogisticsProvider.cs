using System.Collections.Generic;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderLogisticsProviders.Interfaces {
	using Logistics = Logistics.src.Database.Logistics;

	/// <summary>
	/// 创建订单可使用的物流的提供器
	/// </summary>
	public interface IOrderLogisticsProvider {
		/// <summary>
		/// 获取默认可使用的物流
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <param name="sellerId">卖家Id</param>
		/// <param name="logisticsList">物流列表</param>
		void GetLogisticsList(long? userId, long? sellerId, IList<Logistics> logisticsList);
	}
}
