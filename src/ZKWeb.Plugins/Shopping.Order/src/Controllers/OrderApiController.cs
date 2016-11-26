using System;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.Controllers.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.Forms;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Controllers {
	/// <summary>
	/// 订单Api控制器
	/// </summary>
	[ExportMany]
	public class OrderApiController : ControllerBase {
		/// <summary>
		/// 创建订单
		/// 创建后跳到支付页面
		/// 如果创建了多个订单应该跳到合并交易的支付页面
		/// </summary>
		/// <returns></returns>
		[Action("api/order/create", HttpMethods.POST)]
		public IActionResult Create() {
			this.RequireAjaxRequest();
			var form = new CreateOrderForm();
			return new JsonResult(form.Submit());
		}
	}
}
