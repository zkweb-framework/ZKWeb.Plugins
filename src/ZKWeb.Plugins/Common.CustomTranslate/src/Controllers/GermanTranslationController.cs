using ZKWeb.Plugins.Common.CustomTranslate.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Controllers {
	/// <summary>
	/// 德语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class German : CustomTranslationControllerBase {
		public override string Name { get { return "de-DE"; } }
	}
}
