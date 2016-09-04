using ZKWeb.Plugins.Common.GenericClass.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Controllers {
	/// <summary>
	/// 商品分类
	/// </summary>
	[ExportMany]
	public class ProductClassController : GenericClassControllerBase<ProductClassController> {
		public override string Name { get { return "ProductClass"; } }
	}
}
