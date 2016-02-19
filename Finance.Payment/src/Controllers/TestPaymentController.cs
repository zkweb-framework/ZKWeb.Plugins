using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Web.ActionResults;
using ZKWeb.Plugins.Common.Admin.src;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Web.Interfaces;
using ZKWeb.Plugins.Finance.Payment.src.Forms;
using System.Web;

namespace ZKWeb.Plugins.Finance.Payment.src.Controllers {
	/// <summary>
	/// 测试支付用的控制器
	/// 测试流程
	/// 创建交易页面 => 支付页面 => 结果页面
	/// </summary>
	[ExportMany]
	public class TestPaymentController : IController {
		/// <summary>
		/// 创建交易页面
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
	}
}
