using ZKWeb.Plugins.Common.GenericTag.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericTag.src.GenericTags {
	/// <summary>
	/// 默认标签
	/// </summary>
	[ExportMany]
	public class DefaultTag : GenericTagBuilder {
		public override string Name { get { return "DefaultTag"; } }
	}
}
