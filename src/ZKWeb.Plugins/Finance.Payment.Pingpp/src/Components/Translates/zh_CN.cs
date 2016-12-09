using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.Pingpp.src.Components.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "PingppPaymentApi", "Ping++支付" },
			{ "Support pay transactions by pingpp", "支持通过财付通支付交易" },
			{ "Pingpp", "Ping++" },
			{ "TradeSecretKey", "交易密钥" },
			{ "PingppAppId", "App Id" },
			{ "Starts with app_", "以app_开始" },
			{ "You can provide test key or live key", "您可以填写测试密钥或者Live密钥" },
			{ "PingppRsaPublicKey", "Ping++RSA公钥" },
			{ "Starts with -----BEGIN PUBLIC KEY----", "以-----BEGIN PUBLIC KEY----开始" },
			{ "PartnerRsaPrivateKey", "商户RSA私钥" },
			{ "Starts with -----BEGIN RSA PRIVATE KEY-----", "以-----BEGIN RSA PRIVATE KEY-----开始" },
			{ "ReturnDomain", "返回域名" },
			{ "keep empty will use the default domain", "留空时使用默认域名" },
			{ "PaymentChannels", "支付渠道" },
			{ "WeChatOpenId", "微信Open Id" },
			{ "WeChatNoCredit", "微信限制使用信用卡" },
			{ "FqlChildMerchantId", "分期乐子商户编号" },
			{ "BfbRequireLogin", "百度钱包需要登陆" },
			{ "ExtraPaymentArguments", "扩展支付参数" },
			{ "AlipayAppChannel", "支付宝App支付" },
			{ "AlipayWapChannel", "支付宝手机网页支付" },
			{ "AlipayPcDirectChannel", "支付宝电脑网页支付" },
			{ "AlipayQRChannel", "支付宝扫码支付" },
			{ "BaiduPayChannel", "百度钱包App支付" },
			{ "BaiduPayWapChannel", "百度钱包网页支付" },
			{ "UnionPayB2BChannel", "银联企业网银支付" },
			{ "UnionPayChannel", "银联App支付" },
			{ "UnionPayWapChannel", "银联手机网页支付" },
			{ "UnionPayPcChannel", "银联电脑网页支付" },
			{ "WeChatAppChannel", "微信App支付" },
			{ "WeChatPubChannel", "微信公众号支付" },
			{ "WeChatPubQRChannel", "微信公众号扫码支付" },
			{ "WeChatWapChannel", "微信手机网页支付" },
			{ "YeepayWapChannel", "易宝手机网页支付" },
			{ "JingDongPayWapChannel", "京东手机网页支付" },
			{ "FenQiLePayWapChannel", "分期乐支付" },
			{ "QuantGroupPayWapChannel", "量化派支付" },
			{ "CMBWalletChannel", "招行一网通" },
			{ "ApplePayChannel", "Apple Pay" },
			{ "QQPayChannel", "QQ钱包支付" },
			{ "Ping++ Pay", "Ping++支付" },
			{ "Please select payment channel and click pay", "请选择支付渠道并点击支付" },
			{ "Ping++ only support cny payment", "Ping++只支持人民币支付" },
			{ "Transaction amount not matched, excepted amount is {0}, actual amount is {1}",
				"交易金额不匹配，预想值是{0}，实际值是{1}" },
			{ "Verify Ping++ rsa sign failed", "验证Ping++的RSA签名失败" },
			{ "Waiting Payment Result", "等待支付结果" },
			{ "Please wait while the platform returns payment results...",
				"请稍候，正在等待平台返回支付结果……" },
			{ "Please sure you set the webhook url [{0}] on Ping++",
				"请确保您已在Ping++上设置webhook通知到url [{0}]"}
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
