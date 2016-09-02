using ZKWeb.Plugins.Common.CustomTranslate.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Controllers {
	/// <summary>
	/// 西班牙语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Spanish : CustomTranslationControllerBase {
		public override string Name { get { return "es-ES"; } }
	}
}
