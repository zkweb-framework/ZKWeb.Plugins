using System;
using ZKWebStandard.Ioc;
using ZKWeb.Web;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;

namespace ZKWeb.Plugins.Shopping.Order.src.Controllers {
	/// <summary>
	/// 订单控制器
	/// </summary>
	[ExportMany]
	public class OrderController : ControllerBase {
		/// <summary>
		/// 跳转到订单支付页面
		/// </summary>
		/// <returns></returns>
		[Action("order/pay")]
		public IActionResult Pay() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 确认收货
		/// </summary>
		/// <returns></returns>
		[Action("order/confirm")]
		[Action("order/confirm", HttpMethods.POST)]
		public IActionResult Confirm() {
			throw new NotImplementedException();
		}
	}
}
