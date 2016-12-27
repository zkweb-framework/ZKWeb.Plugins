using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductBookmark.src.Domain.Repositories {
	/// <summary>
	/// 商品收藏的仓储
	/// </summary>
	[ExportMany]
	public class ProductBookmarkRepository : RepositoryBase<Entities.ProductBookmark, Guid> {
	}
}
