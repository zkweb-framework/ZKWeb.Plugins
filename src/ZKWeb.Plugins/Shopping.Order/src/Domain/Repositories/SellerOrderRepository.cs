﻿using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Repositories {
	/// <summary>
	/// 卖家订单的仓储
	/// </summary>
	[ExportMany]
	public class SellerOrderRepository : RepositoryBase<SellerOrder, Guid> { }
}
