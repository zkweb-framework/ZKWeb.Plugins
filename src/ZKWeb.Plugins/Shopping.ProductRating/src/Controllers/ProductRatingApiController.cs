using System;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Shopping.ProductRating.src.UIComponents.AjaxTableHandlers;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductRating.src.Controllers {
	/// <summary>
	/// 商品评价的Api控制器
	/// </summary>
	[ExportMany]
	public class ProductRatingApiController : ControllerBase {
		/// <summary>
		/// 搜索指定商品的评价列表
		/// </summary>
		[Action("api/product_rating/search", HttpMethods.POST)]
		public IActionResult Ratings(Guid id) {
			var json = Request.Get<string>("json");
			var request = AjaxTableSearchRequest.FromJson(json);
			var handlers = new ProductRatingTableHandler().WithExtraHandlers();
			var response = request.BuildResponse(handlers);
			return new JsonResult(response);
		}
	}
}
