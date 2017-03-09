using QRCoder;
using System;
using WxPayAPI;
using ZKWeb.Localize;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Enums;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Plugins.Finance.Payment.Wechat.src.Components.PaymentApiHandlers;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

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
			// 获取商户Id等设置
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			PaymentTransaction transaction;
			WechatApiHandler.ApiData apiData;
			using (UnitOfWork.Scope()) {
				transaction = transactionManager.Get(transactionId);
				// 如果交易不存在, 或不能支付则直接返回
				if (transaction == null) {
					throw new ArgumentException(new T("Transaction with id '{0}' not exist", transactionId));
				} else if (!transaction.Check(t => t.IsPayable).First) {
					return;
				}
				apiData = transaction.Api.ExtraData
					.GetOrDefault<WechatApiHandler.ApiData>("ApiData") ?? new WechatApiHandler.ApiData();
			}
			var config = new WxPayConfig(apiData);
			// 调用支付宝Api查询交易状态
			var request = new WxPayData();
			request.SetValue("out_trade_no", transaction.Serial); // 交易流水号
			var response = WxPayApi.OrderQuery(config, request);
			if (response.IsError) {
				// 出错时设置交易错误
				transactionManager.SetLastError(transactionId, new T(
					"Call wechat order query api failed, {0}", response.ErrMsg));
				return;
			}
			// 如果交易状态是未支付则不进行处理
			var tradeState = response.GetValue("trade_state") as string;
			if (tradeState == "NOTPAY" || string.IsNullOrEmpty(tradeState)) {
				return;
			}
			// 检查金额是否一致, 不一致时中止交易
			var tradeNo = response.GetValue("transaction_id") as string; // 微信上的订单编号
			var paidAmount = response.GetValue("total_fee"); // 支付的金额
			if (transaction.Amount != paidAmount.ConvertOrDefault<decimal>() / 100.0M) {
				transactionManager.Process(transactionId, tradeNo, PaymentTransactionState.Aborted);
				transactionManager.SetLastError(transactionId, new T(
					"Transaction amount not matched, excepted '{0}' but actual '{1}'",
					transaction.Amount, paidAmount));
				return;
			}
			// 根据返回状态处理交易
			if (tradeState == "SUCCESS") {
				// 交易成功
				transactionManager.Process(transactionId, tradeNo, PaymentTransactionState.Success);
			} else if (tradeState == "REFUND" || tradeState == "REVOKED" || tradeState == "PAYERROR") {
				// 交易中止
				transactionManager.Process(transactionId, tradeNo, PaymentTransactionState.Aborted);
				transactionManager.SetLastError(transaction.Id, new T("Buyer closed transaction on wechat"));
			} else if (tradeState == "USERPAYING" || tradeState == "PAYERROR") {
				// 支付中
				transactionManager.Process(transactionId, tradeNo, PaymentTransactionState.WaitingPaying);
			} else {
				// 交易中止, 未知的状态
				transactionManager.Process(transactionId, tradeNo, PaymentTransactionState.Aborted);
				transactionManager.SetLastError(transactionId, new T(
					"Unknown wechat trade status '{0}'", tradeState));
			}
		}

		/// <summary>
		/// 处理微信返回的异步通知
		/// </summary>
		/// <param name="xml">微信发来的xml内容</param>
		/// <param name="transactionId">对应的交易Id</param>
		public virtual void ProcessNotify(string xml, out Guid transactionId) {
			// 解析xml判断是否错误
			var logManager = Application.Ioc.Resolve<LogManager>();
			var notifyData = new WxPayData();
			notifyData.FromXmlWithoutCheckSign(xml);
			if (notifyData.IsError) {
				transactionId = Guid.Empty;
				throw new WxPayException(notifyData.ErrMsg);
			}
			// 获取交易和接口，检查金额是否一致
			PaymentTransaction transaction;
			PaymentApi api;
			WechatApiHandler.ApiData apiData;
			var outTradeNo = notifyData.GetValue("out_trade_no") as string; // 交易流水号
			var tradeNo = notifyData.GetValue("transaction_id") as string; // 微信上的订单编号
			var paidAmount = notifyData.GetValue("total_fee"); // 支付的金额
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			using (UnitOfWork.Scope()) {
				transaction = transactionManager.Get(t => t.Serial == outTradeNo);
				if (transaction == null) {
					throw new ArgumentException(new T("Transaction with serial '{0}' not exist", outTradeNo));
				} else if (transaction.Amount != paidAmount.ConvertOrDefault<decimal>() / 100.0M) {
					throw new ArgumentException(new T(
						"Transaction amount not matched, excepted '{0}' but actual is '{1}'",
						transaction.Amount, paidAmount));
				}
				transactionId = transaction.Id;
				api = transaction.Api;
				apiData = transaction.Api.ExtraData
					.GetOrDefault<WechatApiHandler.ApiData>("ApiData") ??
					new WechatApiHandler.ApiData();
			}
			var config = new WxPayConfig(apiData);
			// 检查签名是否合法
			notifyData.CheckSign(config);
			// 根据返回状态处理交易
			var tradeState = notifyData.GetValue("trade_state") as string;
			if (tradeState == "SUCCESS") {
				// 交易成功
				transactionManager.Process(transactionId, tradeNo, PaymentTransactionState.Success);
			} else if (tradeState == "REFUND" || tradeState == "REVOKED" || tradeState == "PAYERROR") {
				// 交易中止
				transactionManager.Process(transactionId, tradeNo, PaymentTransactionState.Aborted);
				transactionManager.SetLastError(transaction.Id, new T("Buyer closed transaction on wechat"));
			} else if (tradeState == "NOTPAY" || string.IsNullOrEmpty(tradeState)) {
				// 未支付
			} else if (tradeState == "USERPAYING" || tradeState == "PAYERROR") {
				// 支付中
				transactionManager.Process(transactionId, tradeNo, PaymentTransactionState.WaitingPaying);
			} else {
				// 交易中止, 未知的状态
				transactionManager.Process(transactionId, tradeNo, PaymentTransactionState.Aborted);
				transactionManager.SetLastError(transactionId, new T(
					"Unknown wechat trade status '{0}'", tradeState));
			}
		}
	}
}
