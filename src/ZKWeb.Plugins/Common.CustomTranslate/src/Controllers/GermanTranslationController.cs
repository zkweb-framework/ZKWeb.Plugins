using ZKWeb.Plugins.Common.CustomTranslate.src.Controllers.Bases;
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
