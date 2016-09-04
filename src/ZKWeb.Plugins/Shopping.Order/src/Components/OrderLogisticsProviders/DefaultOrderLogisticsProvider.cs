using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Shopping.Logistics.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderLogisticsProviders.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderLogisticsProviders {
	using Logistics = Logistics.src.Domain.Entities.Logistics;

	/// <summary>
	/// 默认的创建订单可使用的物流提供器
	/// </summary>
	[ExportMany]
	public class DefaultOrderLogisticsProvider : IOrderLogisticsProvider {
		/// <summary>
		/// 获取可使用的物流
		/// 后台添加的物流 + 卖家添加的物流
		/// </summary>
		public void GetLogisticsList(Guid? userId, Guid? sellerId, IList<Logistics> logisticsList) {
			var logisticsManager = Application.Ioc.Resolve<LogisticsManager>();
			logisticsList.AddRange(logisticsManager.GetManyWithCache(null));
			if (sellerId != null) {
				logisticsList.AddRange(logisticsManager.GetManyWithCache(sellerId));
			}
		}
	}
}
