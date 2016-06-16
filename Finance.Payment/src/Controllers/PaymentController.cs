using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Forms;
using ZKWeb.Plugins.Finance.Payment.src.Managers;
using ZKWeb.Plugins.Finance.Payment.src.PaymentApiHandlers;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web;

namespace ZKWeb.Plugins.Finance.Payment.src.Controllers {
	/// <summary>
	/// 支付相关的控制器
	/// </summary>
	[ExportMany]
	public class PaymentController : IController {
		/// <summary>
		/// 创建测试交易的页面
		/// </summary>
		/// <returns></returns>
		[Action("admin/payment_apis/test_payment")]
		[Action("admin/payment_apis/test_payment", HttpMethods.POST)]
		public IActionResult TestPayment() {
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(UserTypesGroup.Admin, "PaymentApiManage:Test");
			var form = new TestPaymentForm();
			if (HttpManager.CurrentContext.Request.HttpMethod == HttpMethods.GET) {
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
			privilegeManager.Check(UserTypesGroup.Admin, "PaymentApiManage:Test");
			var id = HttpManager.CurrentContext.Request.Get<long>("id");
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
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			var id = HttpManager.CurrentContext.Request.Get<long>("id");
			var html = transactionManager.GetPaymentHtml(id);
			return new TemplateResult("finance.payment/transaction_pay.html", new { html });
		}

		/// <summary>
		/// 交易的支付结果页面
		/// 这里不检查用户登录，在交易管理器中检查
		/// </summary>
		/// <returns></returns>
		[Action("payment/transaction/pay_result")]
		public IActionResult PayResult() {
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			var id = HttpManager.CurrentContext.Request.Get<long>("id");
			var html = transactionManager.GetResultHtml(id);
			return new TemplateResult("finance.payment/transaction_pay_result.html", new { html });
		}
	}
}
