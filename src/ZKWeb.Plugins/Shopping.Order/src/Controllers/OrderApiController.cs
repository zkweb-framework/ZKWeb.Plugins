using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Web;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Controllers {
	/// <summary>
	/// 订单Api控制器
	/// </summary>
	[ExportMany]
	public class OrderApiController : ControllerBase {
		/// <summary>
		/// 创建订单
		/// </summary>
		/// <returns></returns>
		[Action("api/order/create", HttpMethods.POST)]
		public IActionResult Create() {
			HttpRequestChecker.RequieAjaxRequest();
			var orderManager = Application.Ioc.Resolve<OrderManager>();
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var createOrderParameters = HttpManager.CurrentContext.Request
				.Get<CreateOrderParameters>("CreateOrderParameters") ?? new CreateOrderParameters();
			createOrderParameters.SetLoginInfo();
			var result = orderManager.CreateOrder(createOrderParameters);
			var transaction = result.CreatedTransactions.Last();
			var paymentUrl = transactionManager.GetPaymentUrl(transaction.Id);
			return new JsonResult(new { script = ScriptStrings.Redirect(paymentUrl) });
		}
	}
}
