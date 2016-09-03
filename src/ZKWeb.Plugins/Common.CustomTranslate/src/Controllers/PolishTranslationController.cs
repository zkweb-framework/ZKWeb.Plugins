using ZKWeb.Plugins.Common.CustomTranslate.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Controllers {
	/// <summary>
	/// 波兰语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Polish : CustomTranslationControllerBase {
		public override string Name { get { return "pl-PL"; } }
	}
}
