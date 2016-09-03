using ZKWeb.Plugins.Common.CustomTranslate.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Controllers {
	/// <summary>
	/// 法语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class French : CustomTranslationControllerBase {
		public override string Name { get { return "fr-FR"; } }
	}
}
