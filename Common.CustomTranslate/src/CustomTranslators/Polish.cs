using ZKWeb.Plugins.Common.CustomTranslate.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.CustomTranslators {
	/// <summary>
	/// 波兰语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Polish : CustomTranslator {
		public override string Name { get { return "pl-PL"; } }
	}
}
