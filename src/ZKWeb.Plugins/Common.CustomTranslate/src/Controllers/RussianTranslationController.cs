using ZKWeb.Plugins.Common.CustomTranslate.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Controllers {
	/// <summary>
	/// 俄语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Russian : CustomTranslationControllerBase {
		public override string Name { get { return "ru-RU"; } }
	}
}
