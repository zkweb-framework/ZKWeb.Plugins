#if !NETCORE
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Util;
using Newtonsoft.Json;
using QRCoder;
#endif
using System;
using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Base.src.Components.Mscorlib.Extensions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWeb.Plugins.Finance.Payment.src.Components.Utils;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Enums;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Extensions;
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
		public const string AlipayQRCodePayNotifyUrl = "/payment/alipay_qrcode_pay/notify";

#if !NETCORE
		/// <summary>
		/// 获取支付宝开放平台的客户端实例
		/// </summary>
		/// <param name="apiData">接口数据</param>
		/// <returns></returns>
		public virtual IAopClient GetAopClient(ApiData apiData) {
			var client = new DefaultAopClient(
				UseSandBox ? SandBoxAlipayGatewayUrl : AlipayGatewayUrl,
				apiData.AppId,
				apiData.GetPartnerKeyBody(),
				"json", "1.0", SignType,
				UseSandBox ? SandBoxAlipayPublicKey : AlipayPublicKey);
			return client;
		}

		/// <summary>
		/// 检查支付宝返回的参数签名
		/// </summary>
		/// <param name="parameters">参数列表</param>
		/// <returns></returns>
		public virtual bool CheckSign(SortedDictionary<string, string> parameters) {
			var publicKey = UseSandBox ? SandBoxAlipayPublicKey : AlipayPublicKey;
			var sign = parameters["sign"];
			parameters.Remove("sign");
			parameters.Remove("sign_type");
			// RSACheckV1和RSACheckV2的key参数是文件路径
			// 以下的函数只支持RSA + SHA1
			return AlipaySignature.RSACheckContent(
				AlipaySignature.GetSignContent(parameters), sign, publicKey, "utf-8", false);
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
			// 当前沙箱环境中交易流水号相同时不会重复创建支付宝交易，仅可能会返回不同的二维码
			// 如果正式环境有变化可能需要做出更多处理，例如缓存之前的二维码(并考虑二维码的过期时间)
			var client = GetAopClient(apiData);
			var request = new AlipayTradePrecreateRequest();
			request.BizContent = JsonConvert.SerializeObject(new {
				out_trade_no = transaction.Serial, // 交易流水号
				seller_id = apiData.PayeePartnerId, // 收款商户Id
				total_amount = transaction.Amount.ToString("0.00"), // 金额
				subject = transaction.Description.TruncateWithSuffix(15), // 交易标题
				body = transaction.Description, // 交易描述
			}, Formatting.Indented);
			var notifyUrl = PaymentUtils.GetReturnOrNotifyUrl(
				apiData.ReturnDomain, AlipayQRCodePayNotifyUrl);
			request.SetNotifyUrl(notifyUrl);
			var response = client.Execute(request);
			if (response.IsError) {
				return new HtmlString(templateManager.RenderTemplate(
					"finance.payment/error_text.html",
					new { error = $"{response.Msg}: {response.SubMsg}" }));
			}
			// 生成二维码图片
			var generator = new QRCodeGenerator();
			var qrCodeData = generator.CreateQrCode(response.QrCode, QRCodeGenerator.ECCLevel.Q);
			var qrCode = new Base64QRCode(qrCodeData);
			var qrCodeBase64 = qrCode.GetGraphic(20);
			// 返回模板
			return new HtmlString(templateManager.RenderTemplate(
				"finance.payment.alipaymobile/qrcode_pay.html",
				new { transactionId = transaction.Id, qrCodeBase64 }));
#else
			return new HtmlString(templateManager.RenderTemplate(
				"finance.payment/error_text.html",
				new { error = new T("Alipay qrcode pay is unsupported on .net core yet") }));
#endif
		}

		/// <summary>
		/// 调用支付宝的查询接口查询交易状态
		/// 根据返回结果更新交易
		/// </summary>
		/// <param name="transactionId">交易Id</param>
		/// <returns></returns>
		public virtual void UpdateTransactionState(Guid transactionId) {
#if !NETCORE
			// 获取商户Id等设置
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			PaymentTransaction transaction;
			ApiData apiData;
			using (UnitOfWork.Scope()) {
				transaction = transactionManager.Get(transactionId);
				// 如果交易不存在, 或不能支付则直接返回
				if (transaction == null) {
					throw new ArgumentException(new T("Transaction with id '{0}' not exist", transactionId));
				} else if (!transaction.Check(t => t.IsPayable).First) {
					return;
				}
				apiData = transaction.Api.ExtraData.GetOrDefault<ApiData>("ApiData") ?? new ApiData();
			}
			// 调用支付宝Api查询交易状态
			var client = GetAopClient(apiData);
			var request = new AlipayTradeQueryRequest();
			request.BizContent = JsonConvert.SerializeObject(new {
				out_trade_no = transaction.Serial
			}, Formatting.Indented);
			var response = client.Execute(request);
			if (response.IsError) {
				// 出错时设置交易错误, 除非错误是交易不存在
				// 如果错误是交易不存在代表买家可能尚未支付
				if (response.SubCode != "ACQ.TRADE_NOT_EXIST") {
					transactionManager.SetLastError(transactionId, new T(
						"Call alipay trade query api failed, {0}",
						$"{response.Msg}: {response.SubMsg}"));
				}
				return;
			}
			// 检查金额是否一致, 不一致时中止交易
			var tradeNo = response.TradeNo; // 支付宝上的订单编号
			if (transaction.Amount != response.TotalAmount.ConvertOrDefault<decimal>()) {
				transactionManager.Process(transactionId, tradeNo, PaymentTransactionState.Aborted);
				transactionManager.SetLastError(transactionId, new T(
					"Transaction amount not matched, excepted '{0}' but actual '{1}'",
					transaction.Amount, response.TotalAmount));
				return;
			}
			// 根据返回状态处理交易
			if (response.TradeStatus == "TRADE_SUCCESS" ||
				response.TradeStatus == "TRADE_FINISHED") {
				// 交易成功
				transactionManager.Process(transactionId, tradeNo, PaymentTransactionState.Success);
			} else if (response.TradeStatus == "TRADE_CLOSED") {
				// 交易中止
				transactionManager.Process(transactionId, tradeNo, PaymentTransactionState.Aborted);
				transactionManager.SetLastError(transaction.Id, new T("Buyer closed transaction on alipay"));
			} else if (response.TradeStatus == "WAIT_BUYER_PAY") {
				// 等待付款, 买家已经扫了二维码但是还未支付
				transactionManager.Process(transactionId, tradeNo, PaymentTransactionState.WaitingPaying);
			} else {
				// 交易中止, 未知的状态
				transactionManager.Process(transactionId, tradeNo, PaymentTransactionState.Aborted);
				transactionManager.SetLastError(transactionId, new T(
					"Unknown alipay trade status '{0}'", response.TradeStatus));
			}
#else
			throw new NotSupportedException(
				new T("Alipay mobile pay is unsupported on .net core yet"));
#endif
		}

		/// <summary>
		/// 处理支付宝返回的异步通知
		/// </summary>
		/// <param name="parameters">参数</param>
		/// <param name="transactionId">对应的交易Id</param>
		public virtual void ProcessNotify(
			SortedDictionary<string, string> parameters,
			out Guid transactionId) {
#if !NETCORE
			// 获取、记录和检查参数
			var notifyId = parameters.GetOrDefault("notify_id"); // 通知Id
			var sign = parameters.GetOrDefault("sign"); // 签名字符串
			var outTradeNo = parameters.GetOrDefault("out_trade_no"); // 商户的交易流水号
			var tradeNo = parameters.GetOrDefault("trade_no"); // 支付宝的交易流水号
			var tradeStatus = parameters.GetOrDefault("trade_status"); // 交易状态
			var totalAmount = parameters.GetOrDefault("total_amount"); // 交易金额
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogTransaction(
				$"Process alipay mobile notify " +
				$"allParameters {JsonConvert.SerializeObject(parameters)}");
			if (string.IsNullOrEmpty(notifyId)) {
				throw new ArgumentException("notify_id is empty");
			} else if (string.IsNullOrEmpty(sign)) {
				throw new ArgumentException("sign is empty");
			} else if (string.IsNullOrEmpty(outTradeNo)) {
				throw new ArgumentException("out_trade_no is emtpy");
			} else if (string.IsNullOrEmpty(tradeNo)) {
				throw new ArgumentException("trade_no is empty");
			} else if (string.IsNullOrEmpty(tradeStatus)) {
				throw new ArgumentException("trade_status is empty");
			} else if (string.IsNullOrEmpty(totalAmount)) {
				throw new ArgumentException("total_amount is empty");
			}
			// 获取交易和接口，检查金额是否一致
			PaymentTransaction transaction;
			PaymentApi api;
			ApiData apiData;
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			using (UnitOfWork.Scope()) {
				transaction = transactionManager.Get(t => t.Serial == outTradeNo);
				if (transaction == null) {
					throw new ArgumentException(new T("Transaction with serial '{0}' not exist", outTradeNo));
				} else if (transaction.Amount != totalAmount.ConvertOrDefault<decimal>()) {
					throw new ArgumentException(new T(
						"Transaction amount not matched, excepted '{0}' but actual is '{1}'",
						transaction.Amount, totalAmount));
				}
				api = transaction.Api;
				apiData = api.ExtraData.GetOrDefault<ApiData>("ApiData") ?? new ApiData();
			}
			// 验证消息是否合法
			if (!CheckSign(parameters)) {
				throw new ArgumentException("check alipay sign failed");
			}
			// 处理交易
			var result = Tuple.Create(true, (string)null);
			if (tradeStatus == "TRADE_FINISHED") {
				// 交易已完成且不可退款（不会有后续通知） 
				transactionManager.Process(transaction.Id, tradeNo, PaymentTransactionState.Success);
			} else if (tradeStatus == "TRADE_SUCCESS") {
				// 交易已付款成功
				transactionManager.Process(transaction.Id, tradeNo, PaymentTransactionState.Success);
			} else if (tradeStatus == "WAIT_BUYER_PAY") {
				// 等待买家付款
				transactionManager.Process(transaction.Id, tradeNo, PaymentTransactionState.WaitingPaying);
			} else if (tradeStatus == "TRADE_CLOSED") {
				// 交易中途关闭（已结束，未成功完成）
				// 目前不能获取关闭的原因，只能显示支付宝关闭交易
				transactionManager.Process(transaction.Id, tradeNo, PaymentTransactionState.Aborted);
				transactionManager.SetLastError(transaction.Id, new T("Buyer closed transaction on alipay"));
			} else {
				// 未知的交易状态
				throw new ArgumentException(new T("Unknown alipay trade status '{0}'", tradeStatus));
			}
			// 设置返回的交易Id
			transactionId = transaction.Id;
#else
			throw new NotSupportedException(
				new T("Alipay mobile pay is unsupported on .net core yet"));
#endif
		}
	}
}
