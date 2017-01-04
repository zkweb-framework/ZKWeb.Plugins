using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductRating.src.Domain.Repositories {
	/// <summary>
	/// 商品评价的仓储
	/// </summary>
	[ExportMany]
	public class ProductRatingRepository : RepositoryBase<Entities.ProductRating, Guid> {
	}
}
