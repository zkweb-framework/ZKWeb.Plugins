using ZKWeb.Localize;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Components.Languages {
	/// <summary>
	/// 德语
	/// </summary>
	[ExportMany]
	public class German : ILanguage {
		public string Name { get { return "de-DE"; } }
	}
}
