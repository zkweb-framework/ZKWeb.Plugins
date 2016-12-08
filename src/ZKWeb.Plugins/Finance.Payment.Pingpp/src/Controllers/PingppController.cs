#if !NETCORE
using Pingpp.Models;
using ZKWeb.Plugins.Finance.Payment.Pingpp.src.Domain.Services;
#endif
using System;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Finance.Payment.Pingpp.src.UIComponents.Forms;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Extensions;
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

		/// <summary>
		/// Ping++的后台通知
		/// </summary>
		/// <returns></returns>
		[Action("/payment/pingpp/webhook")]
		[Action("/payment/pingpp/webhook", HttpMethods.POST)]
		public IActionResult WebHook() {
			// 用于给Ping++校验页面存在
			if (Request.Method == HttpMethods.GET) {
				return new PlainResult("Page exists");
			}
#if NETCORE
			throw new BadRequestException("Ping++ on .Net Core is unsupported yet");
#else
			// 获取提交过来的Json
			var jsonBody = Request.GetJsonBody();
			// 获取Http头中的签名
			var signature = Request.GetHeader("x-pingplusplus-signature");
			// 处理消息
			var pingppManager = Application.Ioc.Resolve<PingppManager>();
			pingppManager.ProcessWebHook(jsonBody, signature);
			return new PlainResult("SUCCESS");
#endif
		}
	}
}
