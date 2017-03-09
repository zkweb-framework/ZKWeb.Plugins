using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.WeChat.src.Components.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			// TODO: 翻译到其他语言
			{ "WechatPaymentApi", "微信支付" },
			{ "Support pay transactions by wechat", "支持通过微信支付交易" },
			{ "WechatPay", "微信支付" },
			{ "AppId", "App Id" },
			{ "PartnerId", "商户Id" },
			{ "PartnerKey", "商户密钥" },
			{ "ReturnDomain", "返回域名" },
			{ "keep empty will use the default domain", "留空时使用默认域名" },
			{ "WeChat pay only support CNY", "微信支付只支持人民币" },
			{ "WeChatQRCodePay", "微信扫码支付" },
			{ "Please use wechat scan the following qr code to complete the payment",
				"请使用微信扫描以下的二维码完成支付" },
			{ "Call wechat order query api failed, {0}", "调用微信订单查询Api失败, {0}" },
			{ "Buyer closed transaction on wechat", "买家在微信上关闭了交易" },
			{ "Unknown wechat trade status '{0}'", "未知的微信交易状态'{0}'" },
			{ "Wechat qrcode payment success, redirecting to result page...",
				"微信扫码支付成功，正在跳转到结果页……" },
			{ "Wechat qrcode payment failed, redirecting to result page...",
				"微信扫码支付失败，正在跳转到结果页……" },
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
