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

namespace ZKWeb.Plugins.Shopping.Order.src.Controllers {
	/// <summary>
	/// 订单控制器
	/// </summary>
	[ExportMany]
	public class OrderController : IController {
		/// <summary>
		/// 创建订单
		/// 创建后跳转到支付页面
		/// </summary>
		/// <returns></returns>
		[Action("order/create_order", HttpMethods.POST)]
		public IActionResult CreateOrder() {
			var orderManager = Application.Ioc.Resolve<OrderManager>();
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			var parameters = HttpManager.CurrentContext.Request.Get<CreateOrderParameters>("CreateOrderParameters");
			var result = orderManager.CreateOrder(parameters);
			var transactionId = result.CreatedTransactionIds.Last();
			var paymentUrl = transactionManager.GetPaymentUrl(transactionId);
			return new JsonResult(new { script = ScriptStrings.Redirect(paymentUrl) });
		}

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
