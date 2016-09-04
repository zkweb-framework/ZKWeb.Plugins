using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Repositories {
	/// <summary>
	/// 收货地址的仓储
	/// </summary>
	[ExportMany]
	public class ShippingAddressRepository : RepositoryBase<ShippingAddress, Guid> { }
}
