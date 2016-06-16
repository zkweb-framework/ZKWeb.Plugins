using ZKWeb.Plugins.Common.GenericTag.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.GenericTags {
	/// <summary>
	/// 商品标签
	/// </summary>
	[ExportMany]
	public class ProductTag : GenericTagBuilder {
		public override string Name { get { return "ProductTag"; } }
	}
}
