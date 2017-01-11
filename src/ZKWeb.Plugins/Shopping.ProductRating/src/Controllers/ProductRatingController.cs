using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels;
using ZKWeb.Plugins.Shopping.ProductRating.src.Domain.Services;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductRating.src.Controllers {
	/// <summary>
	/// 商品评价的控制器
	/// </summary>
	[ExportMany]
	public class ProductRatingController : ControllerBase {
		/// <summary>
		/// 商品评价页
		/// </summary>
		/// <param name="serial">订单编号</param>
		/// <returns></returns>
		[Action("user/orders/rate")]
		[Action("user/orders/rate", HttpMethods.POST)]
		public IActionResult Rate(string serial) {
			var sellerOrderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var productRatingManager = Application.Ioc.Resolve<ProductRatingManager>();
			var orderId = sellerOrderManager.GetSellerOrderIdFromSerial(serial);
			if (Request.Method == HttpMethods.GET) {
				var displayInfos = orderId == null ?
					new List<OrderProductDisplayInfo>() :
					productRatingManager.GetUnratedOrderProductDisplayInfos(orderId.Value);
				return new TemplateResult("shopping.productrating/order-rate.html", new { displayInfos });
			}
			return new JsonResult(new { message = "Success" });
		}
	}
}
