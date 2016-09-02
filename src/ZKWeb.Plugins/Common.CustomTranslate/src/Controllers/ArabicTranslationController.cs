using ZKWeb.Plugins.Common.CustomTranslate.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Controllers {
	/// <summary>
	/// 阿拉伯语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Arabic : CustomTranslationControllerBase {
		public override string Name { get { return "ar-DZ"; } }
	}
}
