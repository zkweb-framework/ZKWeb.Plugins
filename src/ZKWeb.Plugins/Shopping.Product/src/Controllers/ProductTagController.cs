using ZKWeb.Plugins.Common.GenericTag.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Controllers {
	/// <summary>
	/// 商品标签
	/// </summary>
	[ExportMany]
	public class ProductTagController : GenericTagControllerBase<ProductTagController> {
		public override string Name { get { return "ProductTag"; } }
	}
}
