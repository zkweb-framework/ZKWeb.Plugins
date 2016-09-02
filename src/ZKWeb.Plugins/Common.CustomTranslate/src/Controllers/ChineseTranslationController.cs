using ZKWeb.Plugins.Common.CustomTranslate.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Controllers {
	/// <summary>
	/// 中文
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Chinese : CustomTranslationControllerBase {
		public override string Name { get { return "zh-CN"; } }
	}
}
