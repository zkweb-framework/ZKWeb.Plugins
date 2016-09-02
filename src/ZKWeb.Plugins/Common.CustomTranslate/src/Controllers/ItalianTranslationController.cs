using ZKWeb.Plugins.Common.CustomTranslate.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Controllers {
	/// <summary>
	/// 意大利语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Italian : CustomTranslationControllerBase {
		public override string Name { get { return "it-IT"; } }
	}
}
