using ZKWeb.Localize;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Components.Languages {
	/// <summary>
	/// 希腊语
	/// </summary>
	[ExportMany]
	public class Greek : ILanguage {
		public string Name { get { return "el-GR"; } }
	}
}
