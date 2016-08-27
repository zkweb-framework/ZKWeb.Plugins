using System;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Admin.src.Domain.Repositories {
	/// <summary>
	/// 用户的仓储
	/// </summary>
	[ExportMany, SingletonReuse]
	public class UserRepository : RepositoryBase<User, Guid> { }
}
