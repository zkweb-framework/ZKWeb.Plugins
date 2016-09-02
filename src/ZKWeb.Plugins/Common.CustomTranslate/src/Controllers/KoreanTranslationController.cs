using ZKWeb.Plugins.Common.CustomTranslate.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Controllers {
	/// <summary>
	/// 韩语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Korean : CustomTranslationControllerBase {
		public override string Name { get { return "ko-KR"; } }
	}
}
