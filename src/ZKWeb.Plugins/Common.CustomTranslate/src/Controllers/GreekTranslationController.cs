using ZKWeb.Plugins.Common.CustomTranslate.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Controllers {
	/// <summary>
	/// 希腊语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Greek : CustomTranslationControllerBase {
		public override string Name { get { return "el-GR"; } }
	}
}
