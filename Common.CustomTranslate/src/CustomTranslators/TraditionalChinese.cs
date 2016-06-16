using ZKWeb.Plugins.Common.CustomTranslate.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.CustomTranslators {
	/// <summary>
	/// 繁体中文
	/// </summary>
	[ExportMany, SingletonReuse]
	public class TraditionalChinese : CustomTranslator {
		public override string Name { get { return "zh-TW"; } }
	}
}
