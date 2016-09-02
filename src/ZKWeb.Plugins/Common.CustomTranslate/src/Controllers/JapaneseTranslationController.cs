using ZKWeb.Plugins.Common.CustomTranslate.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Controllers {
	/// <summary>
	/// 日语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Japanese : CustomTranslationControllerBase {
		public override string Name { get { return "ja-JP"; } }
	}
}
