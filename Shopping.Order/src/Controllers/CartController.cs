using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Currency.src.Managers;
using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWeb.Plugins.Shopping.Order.src.Managers;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web;
using ZKWeb.Web.Abstractions;

namespace ZKWeb.Plugins.Shopping.Order.src.Controllers {
	/// <summary>
	/// 购物车控制器
	/// </summary>
	[ExportMany]
	public class CartController : IController {
		/// <summary>
		/// 购物车页
		/// </summary>
		/// <returns></returns>
		[Action("cart")]
		public IActionResult Index() {
			return new TemplateResult("shopping.order/cart.html");
		}

		/// <summary>
		/// 获取迷你购物车的内容
		/// </summary>
		/// <returns></returns>
		[Action("cart/minicart_contents")]
		public IActionResult MiniCartContents() {
			return new TemplateResult("shopping.order/mini_cart_contents.html");
		}
	}
}
