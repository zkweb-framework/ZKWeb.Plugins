using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.Article.src.Domain.Repositories {
	/// <summary>
	/// 文章的仓储
	/// </summary>
	[ExportMany]
	public class ArticleRepository : RepositoryBase<Entities.Article, Guid> { }
}
