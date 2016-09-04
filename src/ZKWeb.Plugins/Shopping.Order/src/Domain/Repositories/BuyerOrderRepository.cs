using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Repositories {
	/// <summary>
	/// 买家订单的仓储
	/// </summary>
	[ExportMany]
	public class BuyerOrderRepository : RepositoryBase<BuyerOrder, Guid> { }
}
