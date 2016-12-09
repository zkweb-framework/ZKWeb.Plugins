#if !NETCORE
using Newtonsoft.Json;
using Pingpp.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Components.GenericConfigs;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Interfaces;
using ZKWeb.Plugins.Finance.Payment.Pingpp.src.Domain.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Components.Utils;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Enums;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using static ZKWeb.Plugins.Finance.Payment.Pingpp.src.Components.PaymentApiHandlers.PingppApiHandler;

namespace ZKWeb.Plugins.Finance.Payment.Pingpp.src.Domain.Services {
	using Pingpp = global::Pingpp.Pingpp;

	/// <summary>
	/// Ping++管理器
	/// </summary>
	[ExportMany]
	public class PingppManager : DomainServiceBase {
		/// <summary>
		/// 获取支付凭据
		/// </summary>
		/// <param name="transactionId">交易Id</param>
		/// <param name="paymentChannel">支付渠道</param>
		/// <param name="userAgent">客户端的UserAgent</param>
		/// <param name="ipAddress">客户端的Ip地址</param>
		/// <param name="charge">返回的支付凭据</param>
		/// <param name="realResultUrl">返回的结果Url</param
		/// <param name="waitResultUrl">等待返回的结果Url</param>>
		public virtual void GetCharge(
			Guid transactionId, string paymentChannel, string userAgent, string ipAddress,
			out Charge charge, out string realResultUrl, out string waitResultUrl) {
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			PaymentTransaction transaction;
			PaymentApi api;
			ApiData apiData;
			using (UnitOfWork.Scope()) {
				// 获取交易和支付接口
				transaction = transactionManager.Get(transactionId);
				api = transaction == null ? null : transaction.Api;
				// 检查交易和接口是否存在
				if (transaction == null) {
					throw new BadRequestException(new T("Payment transaction not found"));
				} else if (api == null) {
					throw new BadRequestException(new T("Payment api not exist"));
				}
				// 检查当前登录用户是否可以支付
				var result = transaction.Check(c => c.IsPayerLoggedIn);
				if (!result.First) {
					throw new BadRequestException(result.Second);
				}
				result = transaction.Check(c => c.IsPayable);
				if (!result.First) {
					throw new BadRequestException(result.Second);
				}
				apiData = transaction.Api.ExtraData.GetOrDefault<ApiData>("ApiData") ?? new ApiData();
			}
			// 检查货币，虽然Ping++有提供货币参数但是支付渠道里面都只能支付人民币
			if (transaction.CurrencyType != "CNY") {
				throw new BadRequestException(new T("Ping++ only support cny payment"));
			}
			// 获取Ping++的支付凭据
			// 注意这里非线程安全，如果添加多个Ping++接口可能会出问题
			Pingpp.SetApiKey(apiData.TradeSecretKey);
			Pingpp.SetPrivateKey(apiData.PartnerRsaPrivateKey);
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var websiteSettings = configManager.GetData<WebsiteSettings>();
			var extra = new Dictionary<string, object>();
			realResultUrl = PaymentUtils.GetReturnOrNotifyUrl(
				apiData.ReturnDomain,
				transactionManager.GetResultUrl(transactionId));
			waitResultUrl = PaymentUtils.GetReturnOrNotifyUrl(
				apiData.ReturnDomain,
				GetWaitResultUrl(transactionId));
			if (paymentChannel == "alipay_wap" ||
				paymentChannel == "alipay_pc_direct") {
				// 支付宝支付
				extra["success_url"] = waitResultUrl;
			} else if (paymentChannel == "bfb_wap") {
				// 百度钱包支付
				extra["result_url"] = waitResultUrl;
				extra["bfb_login"] = apiData.BfbRequireLogin;
			} else if (paymentChannel == "upacp_wap" ||
				paymentChannel == "upacp_pc") {
				// 银联支付
				extra["result_url"] = waitResultUrl;
			} else if (paymentChannel == "wx" ||
				paymentChannel == "wx_pub" ||
				paymentChannel == "wx_pub_qr" ||
				paymentChannel == "wx_wap") {
				// 微信支付
				if (apiData.WeChatNoCredit) {
					extra["limit_pay"] = "no_credit";
				}
				if (paymentChannel == "wx_pub") {
					extra["open_id"] = apiData.WeChatOpenId;
				} else if (paymentChannel == "wx_pub_qr") {
					extra["product_id"] = transaction.ExtraData
						.GetOrDefault<string>("WeChatProductId") ?? "0";
				} else if (paymentChannel == "wx_wap") {
					extra["result_url"] = waitResultUrl;
				}
			} else if (paymentChannel == "jdpay_wap") {
				// 京东支付
				extra["success_url"] = waitResultUrl;
				extra["fail_url"] = realResultUrl;
			} else if (paymentChannel == "fqlpay_wap") {
				// 分期乐支付
				extra["c_merch_id"] = apiData.FqlChildMerchantId;
				extra["return_url"] = waitResultUrl;
			} else if (paymentChannel == "qgbc_wap") {
				// 量化派支付
				extra["phone"] = transaction.ExtraData
					.GetOrDefault<string>("QgbcBuyerMobile") ?? "0";
				extra["return_url"] = waitResultUrl;
			} else if (paymentChannel == "qpay") {
				// QQ钱包
				var isIOS = userAgent.Contains("iphone") || userAgent.Contains("ipad");
				extra["device"] = isIOS ? "ios" : "android";
			}
			var chargeParams = new Dictionary<string, object>
			{
				{ "order_no", transaction.Serial },
				{ "amount", checked((int)(transaction.Amount * 100)) },
				{ "channel", paymentChannel },
				{ "currency", "cny" },
				{ "subject", $"{new T(websiteSettings.WebsiteName)} {transaction.Serial}" },
				{ "body", transaction.Description },
				{ "client_ip", ipAddress },
				{ "app", new Dictionary<string, string> {{ "id", apiData.PingppAppId }}},
				{ "extra", extra }
			};
			charge = Charge.Create(chargeParams);
		}

		/// <summary>
		/// 处理webhook传入的消息
		/// </summary
		/// <param name="eventJson">消息的Json</param>>
		/// <param name="signature">消息的签名</param>
		public virtual void ProcessWebHook(string eventJson, string signature) {
			// 记录到日志
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogTransaction($"Ping++ webhook receive: ({signature}) {eventJson}");
			// 根据类型处理
			var eventObj = JsonConvert.DeserializeObject<Event>(eventJson);
			if (eventObj.Type == "charge.succeeded") {
				ProcessChargeSuccessed(eventJson, eventObj, signature);
			} else {
				// 忽略处理
			}
		}

		/// <summary>
		/// 处理支付成功
		/// </summary>
		protected virtual void ProcessChargeSuccessed(string eventJson, Event eventObj, string signature) {
			// 获取交易
			var dataObject = eventObj.Data.GetOrDefault<IDictionary<string, object>>("object");
			var serial = dataObject.GetOrDefault<string>("order_no");
			var externalSerial = dataObject.GetOrDefault<string>("transaction_no");
			var amount = dataObject.GetOrDefault<decimal>("amount");
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			PaymentTransaction transaction;
			PaymentApi api;
			ApiData apiData;
			using (UnitOfWork.Scope()) {
				// 获取交易和支付接口
				transaction = transactionManager.Get(t => t.Serial == serial);
				api = transaction == null ? null : transaction.Api;
				// 检查交易和接口是否存在
				if (transaction == null) {
					throw new BadRequestException(new T("Payment transaction not found"));
				} else if (api == null) {
					throw new BadRequestException(new T("Payment api not exist"));
				}
				apiData = transaction.Api.ExtraData.GetOrDefault<ApiData>("ApiData") ?? new ApiData();
			}
			// 检查消息的签名是否正确
			VerifySignedHash(eventJson, signature, apiData.PingppRsaPublicKey);
			// 处理交易已支付，失败时会记录到日志
			if (amount != transaction.Amount * 100) {
				transactionManager.AddDetailRecord(transaction.Id, null,
					new T("Transaction amount not matched, excepted amount is {0}, actual amount is {1}",
					transaction.Amount, amount / 100));
			}
			transactionManager.Process(transaction.Id, externalSerial, PaymentTransactionState.Success);
		}

		/// <summary>
		/// 检查参数的签名
		/// </summary>
		/// <param name="strDataToVerify">要验证的json数据</param>
		/// <param name="strSignedData">签名</param>
		/// <param name="strPublicKey">公钥</param>
		protected virtual void VerifySignedHash(
			string strDataToVerify, string strSignedData, string strPublicKey) {
			byte[] signedData = Convert.FromBase64String(strSignedData);
			UTF8Encoding ByteConverter = new UTF8Encoding();
			byte[] dataToVerify = ByteConverter.GetBytes(strDataToVerify);
			var rsa = new RSACryptoServiceProvider { PersistKeyInCsp = false };
			rsa.LoadPublicKeyPEM(strPublicKey);
			if (!rsa.VerifyData(dataToVerify, "SHA256", signedData)) {
				throw new BadRequestException("Verify Ping++ rsa sign failed");
			}
		}

		/// <summary>
		/// 是否应该继续等待交易结果
		/// </summary>
		/// <param name="transactionId">交易Id</param>
		public virtual bool ShouldWaitResult(Guid transactionId) {
			using (UnitOfWork.Scope()) {
				var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
				var transaction = transactionManager.Get(transactionId);
				if (transaction == null) {
					throw new BadRequestException(new T("Payment transaction not found"));
				}
				return transaction.Check(c => c.IsPayable).First;
			}
		}

		/// <summary>
		/// 获取等待支付结果的Url
		/// </summary>
		/// <param name="transactionId"></param>
		/// <returns></returns>
		public virtual string GetWaitResultUrl(Guid transactionId) {
			return string.Format("/payment/pingpp/wait_result?id={0}", transactionId);
		}
	}
}
#endif
