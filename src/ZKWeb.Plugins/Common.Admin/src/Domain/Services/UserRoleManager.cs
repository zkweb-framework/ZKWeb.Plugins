using System;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Admin.src.Domain.Services {
	/// <summary>
	/// 角色管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class UserRoleManager : DomainServiceBase<UserRole, Guid> { }
}
