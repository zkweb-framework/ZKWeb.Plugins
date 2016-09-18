using System;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.Controllers.Extensions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ScriptStrings;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Extensions;
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
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var createOrderParameters = Request
				.Get<CreateOrderParameters>("CreateOrderParameters") ?? new CreateOrderParameters();
			createOrderParameters.SetLoginInfo();
			var result = orderManager.CreateOrder(createOrderParameters);
			var transaction = result.CreatedTransactions.Last();
			var resultUrl = transactionManager.GetResultUrl(transaction.Id);
			return new JsonResult(new { script = BaseScriptStrings.Redirect(resultUrl) });
		}
	}
}
