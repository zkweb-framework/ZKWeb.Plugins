using ZKWeb.Plugins.Common.CustomTranslate.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Controllers {
	/// <summary>
	/// 英语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class English : CustomTranslationControllerBase {
		public override string Name { get { return "en-US"; } }
	}
}
