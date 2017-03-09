using QRCoder;
using System;
using WxPayAPI;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.Wechat.src.Components.PaymentApiHandlers;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Finance.Payment.WeChat.src.Domain.Services {
	/// <summary>
	/// 微信支付管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class WxPayManager : DomainServiceBase {
		/// <summary>
		/// 获取扫码支付的Html
		/// </summary>
		/// <param name="transaction">支付交易</param>
		/// <returns></returns>
		public virtual HtmlString GetQRCodePaymentHtml(PaymentTransaction transaction) {
			// 获取商户Id等设置
			var apiData = transaction.Api.ExtraData
				.GetOrDefault<WechatApiHandler.ApiData>("ApiData") ??
				new WechatApiHandler.ApiData();
			var config = new WxPayConfig(apiData);
			// 获取货币信息，目前仅支持人民币支付
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			var info = currencyManager.GetCurrency(transaction.CurrencyType);
			if (info == null || info.Type != "CNY") {
				return new HtmlString(templateManager.RenderTemplate(
					"finance.payment/error_text.html",
					new { error = new T("WeChat pay only support CNY") }));
			}
			// 通过微信Api获取二维码Url
			var request = new WxPayData();
			request.SetValue("body", transaction.Description); // 交易描述
			request.SetValue("out_trade_no", transaction.Serial); // 交易流水号
			request.SetValue("total_fee", ((int)(transaction.Amount * 100)).ToString()); // 总金额，单位分
			request.SetValue("trade_type", "NATIVE"); // 扫码支付
			request.SetValue("product_id", transaction.Id.ToString()); // 商品Id, 这里设置交易id
			var response = WxPayApi.UnifiedOrder(config, request);
			if (response.IsError) {
				return new HtmlString(templateManager.RenderTemplate(
					"finance.payment/error_text.html",
					new { error = response.ErrMsg }));
			}
			// 生成二维码图片
			var generator = new QRCodeGenerator();
			var qrCodeUrl = response.GetValue("code_url") as string;
			var qrCodeData = generator.CreateQrCode(qrCodeUrl, QRCodeGenerator.ECCLevel.Q);
			var qrCode = new Base64QRCode(qrCodeData);
			var qrCodeBase64 = qrCode.GetGraphic(20);
			// 返回模板
			return new HtmlString(templateManager.RenderTemplate(
				"finance.payment.wechat/qrcode_pay.html",
				new { transactionId = transaction.Id, qrCodeBase64 }));
		}

		/// <summary>
		/// 调用微信的查询接口查询交易状态
		/// 根据返回结果更新交易
		/// </summary>
		/// <param name="transactionId">交易Id</param>
		/// <returns></returns>
		public virtual void UpdateTransactionState(Guid transactionId) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 处理微信返回的异步通知
		/// </summary>
		/// <param name="context">Http上下文</param>
		/// <param name="transactionId">对应的交易Id</param>
		public virtual void ProcessNotify(IHttpContext context, out Guid transactionId) {
			throw new NotImplementedException();
		}
	}
}
