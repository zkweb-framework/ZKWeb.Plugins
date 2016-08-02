using System.Collections.Generic;
using ZKWeb.Plugins.Shopping.Logistics.src.Manager;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.OrderLogisticsProviders {
	using ZKWebStandard.Extensions;
	using Logistics = Logistics.src.Database.Logistics;

	/// <summary>
	/// 默认的创建订单可使用的物流提供器
	/// </summary>
	[ExportMany]
	public class DefaultOrderLogisticsProvider : IOrderLogisticsProvider {
		/// <summary>
		/// 获取可使用的物流
		/// 后台添加的物流 + 卖家添加的物流
		/// </summary>
		public void GetLogisticsList(long? userId, long? sellerId, IList<Logistics> logisticsList) {
			var logisticsManager = Application.Ioc.Resolve<LogisticsManager>();
			logisticsList.AddRange(logisticsManager.GetLogisticsList(null));
			if (sellerId != null) {
				logisticsList.AddRange(logisticsManager.GetLogisticsList(sellerId));
			}
		}
	}
}
