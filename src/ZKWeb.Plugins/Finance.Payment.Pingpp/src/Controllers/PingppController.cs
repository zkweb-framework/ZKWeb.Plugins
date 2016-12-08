using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Finance.Payment.Pingpp.src.UIComponents.Forms;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.Pingpp.src.Controllers {
	/// <summary>
	/// Ping++支付的控制器
	/// </summary>
	[ExportMany]
	public class PingppController : ControllerBase {
		/// <summary>
		/// 获取Ping++支付凭据并返回给客户端
		/// </summary>
		/// <returns></returns>
		[Action("/payment/pingpp/pay", HttpMethods.POST)]
		public IActionResult Pay() {
			var form = new PingppPayForm();
			return new JsonResult(form.Submit());
		}
	}
}
