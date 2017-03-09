using System;
using System.IO;
using WxPayAPI;
using ZKWeb.Localize;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Base.src.Components.Mscorlib.Extensions;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ScriptStrings;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Enums;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Plugins.Finance.Payment.WeChat.src.Domain.Services;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.WeChat.src.Controllers {
	/// <summary>
	/// 微信控制器
	/// </summary>
	[ExportMany]
	public class WxPayController : ControllerBase {
		/// <summary>
		/// 微信扫码支付的异步通知
		/// </summary>
		/// <returns></returns>
		[Action(WxPayConfig.WeChatQRCodePayNotifyUrl, HttpMethods.GET)]
		[Action(WxPayConfig.WeChatQRCodePayNotifyUrl, HttpMethods.POST)]
		public IActionResult QRCodePayNotify() {
			// 获取收到的xml
			string xml;
			using (var reader = new StreamReader(Request.Body)) {
				xml = reader.ReadToEnd();
			}
			// 处理回应
			var logManager = Application.Ioc.Resolve<LogManager>();
			var wxpayManager = Application.Ioc.Resolve<WxPayManager>();
			var data = new WxPayData();
			try {
				Guid transactionId;
				wxpayManager.ProcessNotify(xml, out transactionId);
				data.SetValue("return_code", "SUCCESS");
				logManager.LogDebug("Process wechat qrcode pay notify success");
			} catch (Exception ex) {
				data.SetValue("return_code", "FAIL");
				data.SetValue("return_msg", ex.Message.TruncateWithSuffix(100));
				logManager.LogDebug($"Process wechat qrcode pay notify failed: {ex}");
			}
			return new PlainResult(data.ToXml()) { ContentType = "text/xml" };
		}

		/// <summary>
		/// 更新扫码支付的交易状态
		/// </summary>
		/// <returns></returns>
		[Action("/payment/wechat_qrcode_pay/update_transaction_state", HttpMethods.POST)]
		public IActionResult UpdateTransactionState(Guid transactionId) {
			// 更新交易状态
			var wxpayManager = Application.Ioc.Resolve<WxPayManager>();
			wxpayManager.UpdateTransactionState(transactionId);
			// 支付成功或失败时跳转到结果页
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			var transaction = transactionManager.Get(transactionId);
			var resultUrl = transactionManager.GetResultUrl(transactionId);
			if (transaction.State == PaymentTransactionState.Success) {
				return new JsonResult(new {
					message = new T("Wechat qrcode payment success, redirecting to result page..."),
					script = BaseScriptStrings.Redirect(resultUrl, 3000)
				});
			} else if (!transaction.Check(t => t.IsPayable).First) {
				return new JsonResult(new {
					message = new T("Wechat qrcode payment failed, redirecting to result page..."),
					script = BaseScriptStrings.Redirect(resultUrl, 3000)
				});
			}
			return new JsonResult(new { hint = "success but nothing changed" });
		}
	}
}
