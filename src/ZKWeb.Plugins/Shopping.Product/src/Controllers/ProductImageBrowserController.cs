using ZKWeb.Plugins.CMS.ImageBrowser.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Controller {
	/// <summary>
	/// 商品图片浏览器
	/// </summary>
	[ExportMany]
	public class ProductImageBrowserController : ImageBrowserControllerBase {
		public override string Category { get { return "Product"; } }
	}
}
