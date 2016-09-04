using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericRecord.src.Domain.Repositories {
	/// <summary>
	/// 通用记录的仓储
	/// </summary>
	[ExportMany]
	public class GenericRecordRepository : RepositoryBase<Entities.GenericRecord, Guid> { }
}
