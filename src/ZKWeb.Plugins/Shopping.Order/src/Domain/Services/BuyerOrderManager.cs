using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Services {
	/// <summary>
	/// 买家订单管理器
	/// 包含买家使用的订单操作
	/// </summary>
	[ExportMany]
	public class BuyerOrderManager : DomainServiceBase<BuyerOrder, Guid> { }
}
