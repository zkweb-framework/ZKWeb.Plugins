using ZKWeb.Plugins.Common.GenericTag.src.UIComponents.ListItemProviders;
using ZKWeb.Plugins.Shopping.Product.src.Controllers;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ListItemProviders {
	/// <summary>
	/// 商品标签选的选项列表提供器
	/// </summary>
	[ExportMany(ContractKey = "ProductTagListItemProvider")]
	public class ProductTagListItemProvider : GenericTagListItemProvider<ProductTagController> {
	}
}
