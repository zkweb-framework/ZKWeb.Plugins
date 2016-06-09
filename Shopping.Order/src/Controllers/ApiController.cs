using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Shopping.Order.src.Managers;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWeb.Utils.IocContainer;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web.Interfaces;

namespace ZKWeb.Plugins.Shopping.Order.src.Controllers {
	/// <summary>
	/// Api控制器
	/// </summary>
	[ExportMany]
	public class ApiController : IController {
		/// <summary>
		/// 获取购物车商品的总数量
		/// </summary>
		/// <returns></returns>
		[Action("api/cart/product_total_count", HttpMethods.POST)]
		public IActionResult CartProductTotalCount() {
			var cartProductManager = Application.Ioc.Resolve<CartProductManager>();
			var totalCount = cartProductManager.GetCartProductTotalCount(CartProductType.Default);
			return new JsonResult(new { totalCount });
		}
	}
}
