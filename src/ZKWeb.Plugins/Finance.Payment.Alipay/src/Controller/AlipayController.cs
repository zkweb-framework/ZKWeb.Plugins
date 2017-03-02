using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Finance.Payment.Alipay.src.Domain.Services;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Collections;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.Alipay.src.Controller {
	/// <summary>
	/// 支付宝功能使用的控制器
	/// </summary>
	[ExportMany]
	public class AlipayController : ControllerBase {
		/// <summary>
		/// 获取请求参数
		/// </summary>
		/// <returns></returns>
		private SortedDictionary<string, string> GetRequestParameters() {
			var parameters = new SortedDictionary<string, string>();
			IEnumerable<Pair<string, IList<string>>> values;
			if (Request.Method == HttpMethods.GET) {
				values = Request.GetQueryValues();
			} else if (Request.Method == HttpMethods.POST) {
				values = Request.GetFormValues();
			} else {
				throw new NotSupportedException($"Unsupported method {Request.Method}");
			}
			foreach (var value in values) {
				parameters.Add(value.First, value.Second[0]);
			}
			return parameters;
		}

		/// <summary>
		/// 支付宝的返回Url
		/// </summary>
		/// <returns></returns>
		[Action(AlipayManager.AlipayReturnUrl, HttpMethods.GET)]
		[Action(AlipayManager.AlipayReturnUrl, HttpMethods.POST)]
		public IActionResult Return() {
			var alipayManager = Application.Ioc.Resolve<AlipayManager>();
			var parameters = GetRequestParameters();
			// 处理回应
			Guid transactionId;
			alipayManager.ProcessReponse(parameters, false, out transactionId);
			// 跳转到结果Url
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			var resultUrl = transactionManager.GetResultUrl(transactionId);
			return new RedirectResult(resultUrl);
		}

		/// <summary>
		/// 支付宝的异步通知Url
		/// </summary>
		/// <returns></returns>
		[Action(AlipayManager.AlipayNotifyUrl, HttpMethods.GET)]
		[Action(AlipayManager.AlipayNotifyUrl, HttpMethods.POST)]
		public IActionResult Notify() {
			var alipayManager = Application.Ioc.Resolve<AlipayManager>();
			var parameters = GetRequestParameters();
			// 处理回应
			Guid transactionId;
			alipayManager.ProcessReponse(parameters, true, out transactionId);
			// 成功时返回success
			return new PlainResult("success");
		}
	}
}
