using ZKWeb.Plugins.Common.CustomTranslate.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Controllers {
	/// <summary>
	/// 泰语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Thai : CustomTranslationControllerBase {
		public override string Name { get { return "th-TH"; } }
	}
}
