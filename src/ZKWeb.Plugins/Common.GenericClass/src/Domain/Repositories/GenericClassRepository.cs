using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericClass.src.Domain.Repositories {
	/// <summary>
	/// 通用分类的仓储
	/// </summary>
	[ExportMany]
	public class GenericClassRepository : RepositoryBase<Entities.GenericClass, Guid> { }
}
