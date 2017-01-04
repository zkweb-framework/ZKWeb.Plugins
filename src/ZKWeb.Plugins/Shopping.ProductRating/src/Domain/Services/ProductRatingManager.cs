using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductRating.src.Domain.Services {
	/// <summary>
	/// 商品评价管理器
	/// </summary>
	[ExportMany]
	public class ProductRatingManager : DomainServiceBase<Entities.ProductRating, Guid> {
	}
}
