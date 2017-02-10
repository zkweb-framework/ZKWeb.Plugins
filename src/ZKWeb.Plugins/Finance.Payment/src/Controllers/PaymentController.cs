using System;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Finance.Payment.src.UIComponents.Forms;
using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentApiHandlers;
using ZKWeb.Plugins.Finance.Payment.src.UIComponents.HtmlProviders;
using System.ComponentModel;

namespace ZKWeb.Plugins.Finance.Payment.src.Controllers {
	/// <summary>
	/// 支付相关的控制器
	/// </summary>
	[ExportMany]
	public class PaymentController : ControllerBase {
		/// <summary>
		/// 创建测试交易的页面
		/// </summary>
		/// <returns></returns>
		[Action("admin/payment_apis/test_payment")]
		[Action("admin/payment_apis/test_payment", HttpMethods.POST)]
		public IActionResult TestPayment() {
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(typeof(IAmAdmin), "PaymentApiManage:Test");
			var form = new TestPaymentForm();
			if (Request.Method == HttpMethods.GET) {
				form.Bind();
				return new TemplateResult("finance.payment/test_payment.html", new { form });
			} else {
				return new JsonResult(form.Submit());
			}
		}

		/// <summary>
		/// 使用测试接口支付指定交易
		/// </summary>
		/// <returns></returns>
		[Action("admin/payment_apis/test_api_pay", HttpMethods.POST)]
		public IActionResult TestApiPay() {
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(typeof(IAmAdmin), "PaymentApiManage:Test");
			var id = Request.Get<Guid>("id");
			var form = new TestApiHandler.TestApiPayForm(id);
			return new JsonResult(form.Submit());
		}

		/// <summary>
		/// 交易的支付页面
		/// 这里不检查用户登录，在交易管理器中检查
		/// </summary>
		/// <returns></returns>
		[Action("payment/transaction/pay")]
		public IActionResult Pay() {
			var transactionHtmlProvider = Application.Ioc.Resolve<PaymentTransactionHtmlProvider>();
			var id = Request.Get<Guid>("id");
			var html = transactionHtmlProvider.GetPaymentHtml(id);
			return new TemplateResult("finance.payment/transaction_pay.html", new { html });
		}

		/// <summary>
		/// 交易的支付结果页面
		/// 这里不检查用户登录，在交易管理器中检查
		/// </summary>
		/// <returns></returns>
		[Action("payment/transaction/pay_result")]
		[Description("PaymentResultPage")]
		public IActionResult PayResult() {
			var transactionHtmlProvider = Application.Ioc.Resolve<PaymentTransactionHtmlProvider>();
			var id = Request.Get<Guid>("id");
			var html = transactionHtmlProvider.GetResultHtml(id);
			return new TemplateResult("finance.payment/transaction_pay_result.html", new { html });
		}
	}
}
