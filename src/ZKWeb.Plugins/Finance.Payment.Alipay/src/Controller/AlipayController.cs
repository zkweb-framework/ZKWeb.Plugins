using System;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Finance.Payment.Alipay.src.Domain.Services;
using ZKWeb.Web;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.Alipay.src.Controller {
	/// <summary>
	/// 支付宝功能使用的控制器
	/// </summary>
	[ExportMany]
	public class AlipayController : ControllerBase {
		/// <summary>
		/// 支付宝的返回Url
		/// </summary>
		/// <returns></returns>
		[Action(AlipayManager.AlipayReturnUrl, HttpMethods.GET)]
		[Action(AlipayManager.AlipayReturnUrl, HttpMethods.POST)]
		public IActionResult Return() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 支付宝的异步通知Url
		/// </summary>
		/// <returns></returns>
		[Action(AlipayManager.AlipayNotifyUrl, HttpMethods.GET)]
		[Action(AlipayManager.AlipayNotifyUrl, HttpMethods.POST)]
		public IActionResult Notify() {
			throw new NotImplementedException();
		}
	}
}
