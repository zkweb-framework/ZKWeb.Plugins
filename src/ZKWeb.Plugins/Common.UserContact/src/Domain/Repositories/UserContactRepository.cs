using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.UserContact.src.Domain.Repositories {
	/// <summary>
	/// 用户联系信息的仓储
	/// </summary>
	[ExportMany]
	public class UserContactRepository : RepositoryBase<Entities.UserContact, Guid> { }
}
