using ZKWeb.Plugins.Common.CustomTranslate.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.CustomTranslators {
	/// <summary>
	/// 韩语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Korean : CustomTranslator {
		public override string Name { get { return "ko-KR"; } }
	}
}
