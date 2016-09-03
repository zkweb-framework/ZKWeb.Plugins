using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Domain.Repositories {
	/// <summary>
	/// 商品属性的仓储
	/// </summary>
	[ExportMany]
	public class ProductPropertyRepository : RepositoryBase<ProductProperty, Guid> { }
}
