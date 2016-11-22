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
	[ExportMany]
	public class BuyerOrderManager : DomainServiceBase<BuyerOrder, Guid> {
		/// <summary>
		/// 根据订单编号获取买家订单Id
		/// 失败时返回null
		/// </summary>
		/// <param name="serial">订单编号</param>
		/// <returns></returns>
		public Guid? GetBuyerOrderIdFromSerial(string serial) {
			return Repository.Query().FirstOrDefault(o => o.SellerOrder.Serial == serial)?.Id;
		}
	}
}
