using ZKWeb.Localize;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Languages {
	/// <summary>
	/// 日语
	/// </summary>
	[ExportMany]
	public class Japanese : ILanguage {
		public string Name { get { return "ja-JP"; } }
	}
}
