using ZKWeb.Plugins.Common.GenericClass.src.UIComponents.ListItemProviders;
using ZKWeb.Plugins.Shopping.Product.src.Controllers;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ListItemProviders {
	/// <summary>
	/// 商品分类的选项列表提供器
	/// </summary>
	[ExportMany(ContractKey = "ProductClassListItemTreeProvider")]
	public class ProductClassListItemTreeProvider : GenericClassListItemTreeProvider<ProductClassController> {
	}
}
