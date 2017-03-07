using System;
using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ScriptStrings;
using ZKWeb.Plugins.Finance.Payment.AlipayMobile.src.Domain.Services;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Enums;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
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
		/// 支付宝扫码支付的异步通知
		/// </summary>
		/// <returns></returns>
		[Action(AlipayMobileManager.AlipayQRCodePayNotifyUrl, HttpMethods.GET)]
		[Action(AlipayMobileManager.AlipayQRCodePayNotifyUrl, HttpMethods.POST)]
		public IActionResult QRCodePayNotify() {
			// 处理回应
			var alipayMobileManager = Application.Ioc.Resolve<AlipayMobileManager>();
			Guid transactionId;
			alipayMobileManager.ProcessNotify(GetRequestParameters(), out transactionId);
			// 成功时返回success
			return new PlainResult("success");
		}

		/// <summary>
		/// 更新扫码支付的交易状态
		/// </summary>
		/// <returns></returns>
		[Action("/payment/alipay_qrcode_pay/update_transaction_state", HttpMethods.POST)]
		public IActionResult UpdateTransactionState(Guid transactionId) {
			// 更新交易状态
			var alipayMobileManager = Application.Ioc.Resolve<AlipayMobileManager>();
			alipayMobileManager.UpdateTransactionState(transactionId);
			// 支付成功或失败时跳转到结果页
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			var transaction = transactionManager.Get(transactionId);
			var resultUrl = transactionManager.GetResultUrl(transactionId);
			if (transaction.State == PaymentTransactionState.Success) {
				return new JsonResult(new {
					message = new T("Alipay qrcode payment success, redirecting to result page..."),
					script = BaseScriptStrings.Redirect(resultUrl, 3000)
				});
			} else if (!transaction.Check(t => t.IsPayable).First) {
				return new JsonResult(new {
					message = new T("Alipay qrcode payment failed, redirecting to result page..."),
					script = BaseScriptStrings.Redirect(resultUrl, 3000)
				});
			}
			return new JsonResult(new { hint = "success but nothing changed" });
		}
	}
}
