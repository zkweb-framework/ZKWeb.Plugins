using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Finance.Payment.src.Forms;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web.Interfaces;

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
			PrivilegesChecker.Check(UserTypesGroup.Admin, "PaymentApiManage:Test");
			var form = new TestPaymentForm();
			if (HttpContext.Current.Request.HttpMethod == HttpMethods.GET) {
				form.Bind();
				return new TemplateResult("finance.payment/test_payment.html", new { form });
			} else {
				return new JsonResult(form.Submit());
			}
		}

		/// <summary>
		/// 交易的支付页面
		/// </summary>
		/// <returns></returns>
		[Action("payment/transaction/pay")]
		public IActionResult Pay() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 交易的支付结果页面
		/// </summary>
		/// <returns></returns>
		[Action("payment/transaction/pay_result")]
		public IActionResult PayResult() {
			throw new NotImplementedException();
		}
	}
}
