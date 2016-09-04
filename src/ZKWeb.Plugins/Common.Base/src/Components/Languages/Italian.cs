using ZKWeb.Localize;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Components.Languages {
	/// <summary>
	/// 意大利语
	/// </summary>
	[ExportMany]
	public class Italian : ILanguage {
		public string Name { get { return "it-IT"; } }
	}
}
