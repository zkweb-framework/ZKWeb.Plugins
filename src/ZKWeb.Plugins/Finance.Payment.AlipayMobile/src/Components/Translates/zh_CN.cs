using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.AlipayMobile.src.Components.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			// TODO: 翻译到其他语言
			{ "AlipayMobilePaymentApi", "支付宝移动端支付" },
			{ "Support pay transactions by alipay mobile", "支持在移动端通过支付宝支付交易" },
			{ "AlipayQRCodePay", "支付宝扫码支付" },
			{ "AppId, usually starts with datetime such as 20170306", "AppId, 通常以日期开始例如20170306" },
			{ "Alipay barcode pay is unsupported on .net core yet", "支付宝扫码支付仍未支持.Net Core" },
			{ "Alipay mobile pay is unsupported on .net core yet", "支付宝移动端支付仍未支持.Net Core" },
			{ "Alipay only support CNY", "支付宝只支持人民币支付" },
			{ "PartnerId", "商户Id" },
			{ "PartnerId, usually starts with 2088", "商品Id, 一般以2088开始" },
			{ "PayeePartnerId", "收款商户Id" },
			{ "PayeePartnerId, usually same with PartnerId", "收款商户Id, 一般和商户Id相同" },
			{ "PartnerKey", "商户密钥" },
			{ "PartnerKey (RSA with SHA1), usually starts with -----BEGIN RSA PRIVATE KEY-----",
				"商户密钥 (RSA + SHA1), 一般以-----BEGIN RSA PRIVATE KEY-----开始" },
			{ "Please use alipay scan the following qr code to complete the payment",
				"请使用支付宝扫描以下的二维码完成支付" },
			{ "Call alipay trade query api failed, {0}", "调用支付宝查询交易接口失败, {0}" },
			{ "Transaction amount not matched, excepted '{0}' but actual is '{1}'",
				"支付金额不匹配，预期金额是'{0}'但实际金额是'{1}'" },
			{ "Unknown alipay trade status '{0}'", "不支持的支付宝交易状态: '{0}'" },
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
