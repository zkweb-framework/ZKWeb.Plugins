using ZKWeb.Localize;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Components.Languages {
	/// <summary>
	/// 俄语
	/// </summary>
	[ExportMany]
	public class Russian : ILanguage {
		public string Name { get { return "ru-RU"; } }
	}
}
