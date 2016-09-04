using ZKWeb.Plugins.Common.CustomTranslate.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Controllers {
	/// <summary>
	/// 捷克语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Czech : CustomTranslationControllerBase {
		public override string Name { get { return "cs-CZ"; } }
	}
}
