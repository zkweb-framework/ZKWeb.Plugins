using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.Plugins.Common.CustomTranslate.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Domain.Repositories {
	/// <summary>
	/// 自定义翻译的仓储
	/// </summary>
	[ExportMany]
	public class CustomTranslationRepository : RepositoryBase<CustomTranslation, Guid> { }
}
