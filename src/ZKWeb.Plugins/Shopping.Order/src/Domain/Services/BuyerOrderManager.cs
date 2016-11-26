using System;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Services {
	/// <summary>
	/// 买家订单管理器
	/// 包含买家使用的订单操作
	/// </summary>
	[ExportMany, SingletonReuse]
	public class BuyerOrderManager : DomainServiceBase<BuyerOrder, Guid> {
		/// <summary>
		/// 根据订单编号获取买家订单Id
		/// 失败时返回null
		/// </summary>
		/// <param name="serial">订单编号</param>
		/// <returns></returns>
		public virtual Guid? GetBuyerOrderIdFromSerial(string serial) {
			var orderId = Repository.Query()
				.Where(o => o.SellerOrder.Serial == serial)
				.Select(o => o.Id)
				.FirstOrDefault();
			return orderId == Guid.Empty ? null : (Guid?)orderId;
		}

		/// <summary>
		/// 取消订单
		/// </summary>
		/// <param name="orderId">买家订单Id</param>
		/// <param name="operatorId">操作人Id</param>
		/// <param name="reason">作废理由，必填</param>
		/// <returns></returns>
		public virtual bool CancelOrder(Guid orderId, Guid? operatorId, string reason) {
			var sellerOrderId = Repository.Query()
				.Where(o => o.Id == orderId)
				.Select(o => o.SellerOrder.Id)
				.FirstOrDefault();
			var sellerOrderManager = Application.Ioc.Resolve<SellerOrderManager>();
			return sellerOrderManager.CancelOrder(sellerOrderId, operatorId, reason);
		}
	}
}
