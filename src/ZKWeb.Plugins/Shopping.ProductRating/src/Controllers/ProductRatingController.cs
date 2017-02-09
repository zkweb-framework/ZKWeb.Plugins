using System;
using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Components.ActionFilters;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ScriptStrings;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels;
using ZKWeb.Plugins.Shopping.ProductRating.src.Domain.Services;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Extensions;
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
		[CheckOwner]
		public IActionResult Rate(string serial) {
			var buyerOrderManager = Application.Ioc.Resolve<BuyerOrderManager>();
			var productRatingManager = Application.Ioc.Resolve<ProductRatingManager>();
			var buyerOrderId = buyerOrderManager.GetBuyerOrderIdFromSerial(serial);
			if (Request.Method == HttpMethods.GET) {
				var displayInfos = buyerOrderId == null ?
					new List<OrderProductDisplayInfo>() :
					productRatingManager.GetUnratedOrderProductDisplayInfos(buyerOrderId.Value);
				return new TemplateResult("shopping.productrating/order_rate.html", new { displayInfos });
			}
			// 提交评价
			var arguments = Request.GetAllDictionary();
			productRatingManager.AddRatingsFromRequestArguments(buyerOrderId.Value, arguments);
			return new JsonResult(new {
				message = new T("Rating successful, Redirecting to order list..."),
				script = BaseScriptStrings.Redirect("/user/orders", 1500)
			});
		}
	}
}
