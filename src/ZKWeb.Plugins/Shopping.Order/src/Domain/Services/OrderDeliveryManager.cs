using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Services {
	/// <summary>
	/// 订单发货管理器
	/// </summary>
	[ExportMany]
	public class OrderDeliveryManager : DomainServiceBase<OrderDelivery, Guid> {
		public virtual void DeliveryGoods() {
			throw new NotImplementedException();
		}

		public virtual void NotifyPaymentApiAllGoodsShipped() {
			throw new NotImplementedException();
		}
	}
}
