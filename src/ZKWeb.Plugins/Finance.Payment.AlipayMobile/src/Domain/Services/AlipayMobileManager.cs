#if !NETCORE
using Aop.Api;
using Aop.Api.Request;
using Newtonsoft.Json;
using QRCoder;
#endif
using System;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.Mscorlib.Extensions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWeb.Plugins.Finance.Payment.src.Components.Utils;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Enums;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using static ZKWeb.Plugins.Finance.Payment.AlipayMobile.src.Components.PaymentApiHandlers.AlipayQRCodeApiHandler;

namespace ZKWeb.Plugins.Finance.Payment.AlipayMobile.src.Domain.Services {
	/// <summary>
	/// 移动端支付宝管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class AlipayMobileManager : DomainServiceBase {
		/// <summary>
		/// 是否使用沙箱
		/// </summary>
		public static bool UseSandBox = true;
		/// <summary>
		/// 签名类型, 目前使用的是RSA + SHA1
		/// </summary>
		public static string SignType = "RSA";
		/// <summary>
		/// 支付宝开放平台网关的Url
		/// </summary>
		public static string AlipayGatewayUrl = "https://openapi.alipay.com/gateway.do";
		/// <summary>
		/// 沙箱支付宝开放平台网关的Url
		/// </summary>
		public static string SandBoxAlipayGatewayUrl = "https://openapi.alipaydev.com/gateway.do";
		/// <summary>
		/// 支付宝的RSA公钥
		/// </summary>
		public static string AlipayPublicKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDDI6d306Q8fIfCOaTXyiUeJHkrIvYISRcc73s3vF1ZT7XN8RNPwJxo8pWaJMmvyTn9N4HQ632qJBVHf8sxHi/fEsraprwCtzvzQETrNRwVxLO5jVmRGi60j8Ue1efIlzPXV9je9mkjzOmdssymZkh2QhUrCmZYI/FCEa3/cNMW0QIDAQAB";
		/// <summary>
		/// 沙箱支付宝的RSA公钥
		/// </summary>
		public static string SandBoxAlipayPublicKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDIgHnOn7LLILlKETd6BFRJ0GqgS2Y3mn1wMQmyh9zEyWlz5p1zrahRahbXAfCfSqshSNfqOmAQzSHRVjCqjsAw1jyqrXaPdKBmr90DIpIxmIyKXv4GGAkPyJ/6FTFY99uhpiq0qadD/uSzQsefWo0aTvP/65zi3eof7TcZ32oWpwIDAQAB";
		/// <summary>
		/// 支付宝扫码支付的异步通知Url
		/// </summary>
		public const string AlipayBarcodePayNotifyUrl = "/payment/alipay_barcode_pay/notify";

#if !NETCORE
		/// <summary>
		/// 获取支付宝开放平台的客户端实例
		/// </summary>
		/// <param name="apiData">接口数据</param>
		/// <returns></returns>
		public virtual IAopClient GetAopClient(ApiData apiData) {
			var key = apiData.PartnerKey
				.Replace("-----BEGIN RSA PRIVATE KEY-----", "")
				.Replace("-----END RSA PRIVATE KEY-----", "")
				.Trim();
			var client = new DefaultAopClient(
				UseSandBox ? SandBoxAlipayGatewayUrl : AlipayGatewayUrl,
				apiData.AppId,
				key,
				"json", "1.0", SignType,
				UseSandBox ? SandBoxAlipayPublicKey : AlipayPublicKey);
			return client;
		}
#endif

		/// <summary>
		/// 获取扫码支付的Html
		/// </summary>
		/// <param name="transaction">支付交易</param>
		/// <returns></returns>
		public virtual HtmlString GetQRCodePaymentHtml(PaymentTransaction transaction) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
#if !NETCORE
			// 获取商户Id等设置
			var apiData = transaction.Api.ExtraData.GetOrDefault<ApiData>("ApiData") ?? new ApiData();
			// 获取货币信息，目前仅支持人民币支付
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			var info = currencyManager.GetCurrency(transaction.CurrencyType);
			if (info == null || info.Type != "CNY") {
				return new HtmlString(templateManager.RenderTemplate(
					"finance.payment/error_text.html",
					new { error = new T("Alipay only support CNY") }));
			}
			// 如果交易之前没有获取过二维码Url，则通过支付宝Api获取二维码Url
			var client = GetAopClient(apiData);
			var request = new AlipayTradePrecreateRequest();
			var parameters = request.GetParameters();
			request.BizContent = JsonConvert.SerializeObject(new {
				out_trade_no = transaction.Serial, // 交易流水号
				seller_id = apiData.PayeePartnerId, // 收款商户Id
				total_amount = transaction.Amount.ToString("0.00"), // 金额
				subject = transaction.Description.TruncateWithSuffix(15), // 交易标题
				body = transaction.Description, // 交易描述
			}, Formatting.Indented);
			var notifyUrl = PaymentUtils.GetReturnOrNotifyUrl(
				apiData.ReturnDomain, AlipayBarcodePayNotifyUrl);
			request.SetNotifyUrl(notifyUrl);
			var response = client.Execute(request);
			if (response.IsError) {
				return new HtmlString(templateManager.RenderTemplate(
					"finance.payment/error_text.html",
					new { error = $"{response.Msg}: {response.SubMsg}" }));
			}
			// 更新二维码地址和外部序列号
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			transaction.ExtraData["QrCode"] = response.QrCode;
			transactionManager.Save(ref transaction);
			transactionManager.Process(
				transaction.Id, response.OutTradeNo, PaymentTransactionState.WaitingPaying);
			// 生成二维码图片
			var generator = new QRCodeGenerator();
			var qrCodeData = generator.CreateQrCode(response.QrCode, QRCodeGenerator.ECCLevel.Q);
			var qrCode = new Base64QRCode(qrCodeData);
			var qrCodeBase64 = qrCode.GetGraphic(20);
			// 返回模板
			return new HtmlString(templateManager.RenderTemplate(
				"finance.payment.alipaymobile/qrcode_pay.html", new { qrCodeBase64 }));
#else
			return new HtmlString(templateManager.RenderTemplate(
				"finance.payment/error_text.html",
				new { error = new T("Alipay barcode pay is unsupported on .net core yet") }));
#endif
		}

		/// <summary>
		/// 调用支付宝的查询接口查询交易状态
		/// 根据返回结果更新交易
		/// 返回更新后的交易
		/// </summary>
		/// <param name="transactionId">交易Id</param>
		/// <returns></returns>
		public virtual PaymentTransaction UpdateTransactionState(Guid transactionId) {
#if !NETCORE
			throw new NotImplementedException();
#else
			throw new NotSupportedException(
				new T("Alipay barcode pay is unsupported on .net core yet"));
#endif
		}
	}
}
