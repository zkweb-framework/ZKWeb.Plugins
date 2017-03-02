#if !NETCORE
using Com.Alipay;
#endif
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ZKWeb.Localize;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Base.src.Components.Mscorlib.Extensions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWeb.Plugins.Finance.Payment.Alipay.src.Components.PaymentApiHandlers;
using ZKWeb.Plugins.Finance.Payment.src.Components.Utils;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Enums;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using static ZKWeb.Plugins.Finance.Payment.Alipay.src.Components.PaymentApiHandlers.AlipayApiHandler;

namespace ZKWeb.Plugins.Finance.Payment.Alipay.src.Domain.Services {
	/// <summary>
	/// 支付宝管理器
	/// </summary>
	[ExportMany]
	public class AlipayManager : DomainServiceBase {
		/// <summary>
		/// 支付宝的返回Url
		/// </summary>
		public const string AlipayReturnUrl = "/payment/alipay/return";
		/// <summary>
		/// 支付宝的异步通知Url
		/// </summary>
		public const string AlipayNotifyUrl = "/payment/alipay/notify";

		/// <summary>
		/// 获取支付Html
		/// </summary>
		/// <param name="transaction">支付交易</param>
		/// <returns></returns>
		public virtual HtmlString GetPaymentHtml(PaymentTransaction transaction) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
#if !NETCORE
			// 获取商户Id等设置
			var apiData = transaction.Api.ExtraData.GetOrDefault<ApiData>("ApiData") ?? new ApiData();
			// 获取即时和异步返回地址
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			var returnUrl = PaymentUtils.GetReturnOrNotifyUrl(apiData.ReturnDomain, AlipayReturnUrl);
			var notifyUrl = PaymentUtils.GetReturnOrNotifyUrl(apiData.ReturnDomain, AlipayNotifyUrl);
			// 获取货币信息，目前仅支持人民币支付
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			var info = currencyManager.GetCurrency(transaction.CurrencyType);
			if (info == null || info.Type != "CNY") {
				return new HtmlString(templateManager.RenderTemplate(
					"finance.payment/error_text.html",
					new { error = new T("Alipay only support CNY") }));
			}
			// 设置参数
			// 这里没有设置的参数有
			// anti_phishing_key 防钓鱼时间戳 目前不支持
			// exter_invoke_ip 客户端的IP地址 目前不支持, 防止多IP的用户不能使用
			// receive_name, receive_address, receive_zip, receive_phone, receive_mobile 目前不支持
			var parameters = new SortedDictionary<string, string>();
			parameters["partner"] = apiData.PartnerId; // 商户Id
			parameters["seller_id"] = apiData.PayeePartnerId; // 收款支付宝账号, 一般情况下收款账号就是签约账号
			parameters["_input_charset"] = Config.input_charset; ; // 请求编码，默认是utf-8
			parameters["payment_type"] = Config.payment_type; // 支付类型，必须是1
			parameters["notify_url"] = notifyUrl; // 异步通知url
			parameters["return_url"] = returnUrl; // 同步返回url
			parameters["anti_phishing_key"] = Config.anti_phishing_key; // 防钓鱼时间戳, 目前不使用
			parameters["exter_invoke_ip"] = Config.exter_invoke_ip; // 客户端的IP地址, 目前不使用
			parameters["out_trade_no"] = transaction.Serial; // 交易流水号
			parameters["subject"] = transaction.Description.TruncateWithSuffix(15); // 交易标题
			parameters["body"] = transaction.Description; // 交易描述
			parameters["show_url"] = new Uri(returnUrl).GetLeftPart(UriPartial.Authority); // 商品展示地址，这里使用网站首页
			if (apiData.ServiceType == (int)AlipayServiceTypes.ImmediateArrival) {
				// 即时到账
				parameters["service"] = "create_direct_pay_by_user"; // 服务类型 即时到账
				parameters["total_fee"] = transaction.Amount.ToString("0.00"); // 金额
			} else if (apiData.ServiceType == (int)AlipayServiceTypes.SecuredTransaction ||
				apiData.ServiceType == (int)AlipayServiceTypes.DualInterface) {
				// 担保交易或双接口
				if (apiData.ServiceType == (int)AlipayServiceTypes.SecuredTransaction) {
					parameters["service"] = "create_partner_trade_by_buyer"; // 服务类型 担保交易
				} else {
					parameters["service"] = "trade_create_by_buyer"; // 服务类型 双接口
				}
				parameters["price"] = transaction.Amount.ToString("0.00"); // 金额
				parameters["quantity"] = "1"; // 商品数量，支付宝推荐为1
				parameters["logistics_fee"] = "0"; // 物流费用，这套系统不支持单独传入物流费用
				parameters["logistics_type"] = "EXPRESS"; // 快递（可选EXPRESS, POST, EMS）
				parameters["logistics_payment"] = "SELLER_PAY"; // 因为物流费用不单独算，只能用卖家承担运费
			}
			// 创建并返回支付Html
			var submit = new Submit(apiData.PartnerId, apiData.PartnerKey);
			var html = submit.BuildRequest(parameters, "get", "submit");
			return new HtmlString(html);
#else
			return new HtmlString(templateManager.RenderTemplate(
					"finance.payment/error_text.html",
					new { error = new T("Alipay is unsupported on .net core yet") }));
#endif
		}

		/// <summary>
		/// 调用支付宝的发货接口
		/// </summary>
		/// <param name="transaction">支付交易</param>
		/// <param name="logisticsName">物流名称</param>
		/// <param name="invoiceNo">快递单编号</param>
		public virtual void DeliveryGoods(
			PaymentTransaction transaction, string logisticsName, string invoiceNo) {
#if !NETCORE
			// 获取商户Id等设置
			var apiData = transaction.Api.ExtraData.GetOrDefault<ApiData>("ApiData") ?? new ApiData();
			// 设置参数
			var parameters = new SortedDictionary<string, string>();
			parameters["partner"] = apiData.PartnerId; // 商户Id
			parameters["_input_charset"] = "utf-8"; // 请求编码，默认是utf-8
			parameters["service"] = "send_goods_confirm_by_platform"; // 服务类型 发货
			parameters["trade_no"] = transaction.ExternalSerial;
			parameters["logistics_name"] = logisticsName; // 快递或物流名称
			parameters["invoice_no"] = invoiceNo; // 发货单号
			parameters["transport_type"] = "EXPRESS";
			// 执行发货操作
			var submit = new Submit(apiData.PartnerId, apiData.PartnerKey);
			var resultXml = submit.BuildRequest(parameters);
			var resultDoc = new XmlDocument();
			resultDoc.LoadXml(resultXml);
			var result = (resultDoc.DocumentElement.ChildNodes
				.OfType<XmlNode>().ToDictionary(r => r.Name, r => r.InnerText));
			var isSuccess = result.GetOrDefault("is_success");
			var error = result.GetOrDefault("error");
			// 记录发货是否成功
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			if (isSuccess == "T") {
				transactionManager.AddDetailRecord(transaction.Id, null,
					new T("Call alipay send goods api success"));
			} else {
				transactionManager.AddDetailRecord(transaction.Id, null,
					new T("Call alipay send goods api failed: {0}", error));
			}
#endif
		}

		/// <summary>
		/// 处理支付宝的返回结果
		/// </summary>
		/// <param name="parameters">参数列表</param>
		/// <param name="isNotify">是否异步返回的结果</param>
		/// <param name="transactionId">对应的交易Id</param>
		public virtual void ProcessReponse(
			SortedDictionary<string, string> parameters, bool isNotify, out Guid transactionId) {
#if !NETCORE
			// 获取、记录和检查参数
			var notifyId = parameters.GetOrDefault("notify_id"); // 通知Id
			var sign = parameters.GetOrDefault("sign"); // 签名字符串
			var outTradeNo = parameters.GetOrDefault("out_trade_no"); // 商户的交易流水号
			var tradeNo = parameters.GetOrDefault("trade_no"); // 支付宝的交易流水号
			var tradeStatus = parameters.GetOrDefault("trade_status"); // 交易状态
			var refundStatus = parameters.GetOrDefault("refund_status"); // 退款状态，不一定传入
			var totalFee = parameters.GetOrDefault("total_fee"); // 交易金额
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogTransaction(
				$"Process alipay response: isNotify {isNotify} " +
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
			} else if (string.IsNullOrEmpty(totalFee)) {
				throw new ArgumentException("total_fee is empty");
			}
			// 获取交易和接口，检查金额是否一致
			PaymentTransaction transaction;
			PaymentApi api;
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			using (UnitOfWork.Scope()) {
				transaction = transactionManager.Get(t => t.Serial == outTradeNo);
				if (transaction == null) {
					throw new ArgumentException(new T("transaction with serial {0} not exist", outTradeNo));
				} else if (transaction.Amount != totalFee.ConvertOrDefault<decimal>()) {
					throw new ArgumentException(new T(
						"transaction amount not matched: excepted {0} but actual is {1}",
						transaction.Amount, totalFee));
				}
				api = transaction.Api;
			}
			// 验证消息是否合法
			var apiData = api.ExtraData.GetOrDefault<ApiData>("ApiData") ?? new ApiData();
			var notify = new Notify(apiData.PartnerId);
			if (!notify.Verify(parameters, notifyId, sign)) {
				throw new ArgumentException("check alipay sign failed");
			}
			// 处理交易
			var result = Tuple.Create(true, (string)null);
			if (tradeStatus == "TRADE_FINISHED") {
				// 该种交易状态只在两种情况下出现
				// 1、开通了普通即时到账，买家付款成功后。
				// 2、开通了高级即时到账，从该笔交易成功时间算起，过了签约时的可退款时限（如：三个月以内可退款、一年以内可退款等）后。
				// 判断是即时到账还是担保交易：
				// 使用当前订单状态判断，这是是使用支付宝说明文档中的办法，
				// 因为支付宝返回的数据中没有可以分辨是即时到账还是担保交易的数据。
				transactionManager.Process(transaction.Id, tradeNo, PaymentTransactionState.Success);
			} else if (tradeStatus == "TRADE_SUCCESS") {
				// 该种交易状态只在一种情况下出现——开通了高级即时到账，买家付款成功后。
				transactionManager.Process(transaction.Id, tradeNo, PaymentTransactionState.Success);
			} else if (tradeStatus == "WAIT_BUYER_PAY") {
				// 该判断表示买家已在支付宝交易管理中产生了交易记录，但没有付款
				// 只有担保交易和双接口使用
				transactionManager.Process(transaction.Id, tradeNo, PaymentTransactionState.WaitingPaying);
			} else if (tradeStatus == "WAIT_SELLER_SEND_GOODS") {
				// 该判断示买家已在支付宝交易管理中产生了交易记录且付款成功，但卖家没有发货
				// 只有担保交易和双接口使用
				transactionManager.Process(transaction.Id, tradeNo, PaymentTransactionState.SecuredPaid);
			} else if (tradeStatus == "WAIT_BUYER_CONFIRM_GOODS") {
				// 该判断表示卖家已经发了货，但买家还没有做确认收货的操作
				// 只有担保交易和双接口使用
				// 这里不做任何处理
			} else if (tradeStatus == "TRADE_CLOSED") {
				// 交易中途关闭（已结束，未成功完成）
				// 目前不能获取关闭的原因，只能显示支付宝关闭交易
				transactionManager.Process(transaction.Id, tradeNo, PaymentTransactionState.Aborted);
				transactionManager.SetLastError(transaction.Id, new T("Buyer closed transaction on alipay"));
			} else {
				// 未知的交易状态
				throw new ArgumentException(new T("Unsupported alipay trade status: {0}", tradeStatus));
			}
			// 处理退款
			if (refundStatus == "WAIT_SELLER_AGREE") {
				// 等待卖家同意退款
				// 目前只中止交易并显示买家在支付宝上申请了退款
				transactionManager.Process(transaction.Id, tradeNo, PaymentTransactionState.Aborted);
				transactionManager.SetLastError(transaction.Id, new T("Buyer require refund on alipay"));
			} else if (refundStatus == "SELLER_REFUSE_BUYER") {
				// 卖家拒绝退款
				// 这里不做任何处理
			} else if (refundStatus == "WAIT_BUYER_RETURN_GOODS") {
				// 卖家同意退款，等待买家退货
				// 这里不做任何处理
			} else if (refundStatus == "WAIT_SELLER_CONFIRM_GOODS") {
				// 买家已退货，等待卖家收到退货
				// 这里不做任何处理
			} else if (refundStatus == "REFUND_SUCCESS") {
				// 卖家收到退货，退款成功，交易关闭
				// 这里不做任何处理
			} else if (refundStatus == "REFUND_CLOSED") {
				// 退款关闭
				// 这里不做任何处理
			} else if (string.IsNullOrEmpty(refundStatus)) {
				// 没有退款消息
			} else {
				// 未知的退款状态
				throw new ArgumentException(new T("Unsupported alipay refund status: {0}", tradeStatus));
			}
			// 设置返回的交易Id
			transactionId = transaction.Id;
#else
			throw new NotSupportedException("Alipay is unsupported on .net core yet");
#endif
		}
	}
}
