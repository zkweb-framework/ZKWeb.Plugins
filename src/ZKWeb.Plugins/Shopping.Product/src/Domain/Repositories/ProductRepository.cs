using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Domain.Repositories {
	/// <summary>
	/// 商品的仓储
	/// </summary>
	[ExportMany]
	public class ProductRepository : RepositoryBase<Entities.Product, Guid> { }
}
