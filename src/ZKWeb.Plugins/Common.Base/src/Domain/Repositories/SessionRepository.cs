using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Repositories {
	/// <summary>
	/// 会话的仓储
	/// </summary>
	[ExportMany, SingletonReuse]
	public class SessionRepository : RepositoryBase<Session, Guid> { }
}
