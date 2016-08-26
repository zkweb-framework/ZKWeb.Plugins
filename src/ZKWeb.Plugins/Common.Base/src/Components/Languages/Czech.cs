using ZKWeb.Localize;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Components.Languages {
	/// <summary>
	/// 捷克语
	/// </summary>
	[ExportMany]
	public class Czech : ILanguage {
		public string Name { get { return "cs-CZ"; } }
	}
}
