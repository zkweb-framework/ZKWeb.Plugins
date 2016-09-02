using ZKWeb.Plugins.Common.CustomTranslate.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Controllers {
	/// <summary>
	/// 繁体中文
	/// </summary>
	[ExportMany, SingletonReuse]
	public class TraditionalChinese : CustomTranslationControllerBase {
		public override string Name { get { return "zh-TW"; } }
	}
}
