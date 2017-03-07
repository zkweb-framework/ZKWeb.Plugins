using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Finance.Payment.AlipayMobile.src.Domain.Services;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Collections;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.AlipayMobile.src.Controller {
	/// <summary>
	/// 移动端支付宝控制器
	/// </summary>
	[ExportMany]
	public class AlipayMobileController : ControllerBase {
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
		/// 支付宝移动端支付的异步通知
		/// </summary>
		/// <returns></returns>
		[Action(AlipayMobileManager.AlipayBarcodePayNotifyUrl, HttpMethods.GET)]
		[Action(AlipayMobileManager.AlipayBarcodePayNotifyUrl, HttpMethods.POST)]
		public IActionResult Notify() {
			// 处理回应
			var alipayMobileManager = Application.Ioc.Resolve<AlipayMobileManager>();
			alipayMobileManager.ProcessNotify(GetRequestParameters());
			// 成功时返回success
			return new PlainResult("success");
		}
	}
}
