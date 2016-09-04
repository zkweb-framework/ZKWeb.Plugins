using System;
using ZKWebStandard.Ioc;
using ZKWeb.Web;
using ZKWeb.Plugins.Shopping.Order.src.Managers;
using ZKWebStandard.Web;
using ZKWebStandard.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using System.Linq;
using ZKWeb.Web.ActionResults;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Finance.Payment.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.Controllers {
	/// <summary>
	/// 订单控制器
	/// </summary>
	[ExportMany]
	public class OrderController : IController {
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
