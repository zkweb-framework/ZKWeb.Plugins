using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Logistics.src.Domain.Repositories {
	/// <summary>
	/// 物流的仓储
	/// </summary>
	[ExportMany]
	public class LogisticsRepository : RepositoryBase<Entities.Logistics, Guid> { }
}
