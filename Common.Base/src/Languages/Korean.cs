using ZKWeb.Localize;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Languages {
	/// <summary>
	/// 韩语
	/// </summary>
	[ExportMany]
	public class Korean : ILanguage {
		public string Name { get { return "ko-KR"; } }
	}
}
