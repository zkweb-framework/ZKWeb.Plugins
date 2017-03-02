#if !NETCORE
using Com.Alipay;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.Mscorlib.Extensions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWeb.Plugins.Finance.Payment.Alipay.src.Components.PaymentApiHandlers;
using ZKWeb.Plugins.Finance.Payment.src.Components.Utils;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
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
				parameters["total_fee"] = transaction.Amount.ToString(); // 金额
			} else if (apiData.ServiceType == (int)AlipayServiceTypes.SecuredTransaction ||
				apiData.ServiceType == (int)AlipayServiceTypes.DualInterface) {
				// 担保交易或双接口
				if (apiData.ServiceType == (int)AlipayServiceTypes.SecuredTransaction) {
					parameters["service"] = "create_partner_trade_by_buyer"; // 服务类型 担保交易
				} else {
					parameters["service"] = "trade_create_by_buyer"; // 服务类型 双接口
				}
				parameters["price"] = transaction.Amount.Normalize().ToString(); // 金额
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
	}
}
