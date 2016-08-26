using ZKWeb.Localize;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Components.Languages {
	/// <summary>
	/// 中文
	/// </summary>
	[ExportMany]
	public class Chinese : ILanguage {
		public string Name { get { return "zh-CN"; } }
	}
}
