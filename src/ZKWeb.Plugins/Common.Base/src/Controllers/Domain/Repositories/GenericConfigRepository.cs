using ZKWeb.Plugins.Common.Base.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Repositories {
	/// <summary>
	/// 通用配置的仓储
	/// </summary>
	[ExportMany, SingletonReuse]
	public class GenericConfigRepository : RepositoryBase<GenericConfig, string> { }
}
