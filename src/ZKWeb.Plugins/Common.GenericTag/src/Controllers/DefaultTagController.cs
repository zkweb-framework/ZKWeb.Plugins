using ZKWeb.Plugins.Common.GenericTag.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericTag.src.Controllers {
	/// <summary>
	/// 默认标签
	/// </summary>
	[ExportMany]
	public class DefaultTag : GenericTagControllerBase {
		public override string Name { get { return "DefaultTag"; } }
	}
}
