using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web;

namespace ZKWeb.Plugins.Shopping.Product.src.Controllers {
	/// <summary>
	/// 商品控制器
	/// </summary>
	[ExportMany]
	public class ProductController : IController {
		/// <summary>
		/// 前台商品详情页
		/// </summary>
		/// <returns></returns>
		[Action("product/view")]
		public IActionResult View() {
			return new TemplateResult("shopping.product/product_view.html");
		}

		/// <summary>
		/// 前台商品列表页
		/// </summary>
		/// <returns></returns>
		[Action("product/list")]
		public IActionResult List() {
			return new TemplateResult("shopping.product/product_list.html");
		}
	}
}
