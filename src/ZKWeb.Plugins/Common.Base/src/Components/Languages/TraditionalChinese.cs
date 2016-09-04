using ZKWeb.Localize;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Components.Languages {
	/// <summary>
	/// 繁体中文
	/// </summary>
	[ExportMany]
	public class TraditionalChinese : ILanguage {
		public string Name { get { return "zh-TW"; } }
	}
}
