using ZKWeb.Plugins.Common.Base.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Repositories {
	/// <summary>
	/// 定时任务的仓储
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ScheduledTaskRepository : RepositoryBase<ScheduledTask, string> { }
}
