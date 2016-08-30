using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericTag.src.Domain.Repositories {
	/// <summary>
	/// 通用标签的仓储
	/// </summary>
	[ExportMany]
	public class GenericTagRepository : RepositoryBase<Entities.GenericTag, Guid> { }
}
