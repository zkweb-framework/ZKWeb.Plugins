using System;
using System.Net;
using System.Net.Sockets;
using ZKWeb.Plugins.Common.Base.src.Components.Mscorlib.Utils;
using ZKWeb.Plugins.Finance.Payment.src.Components.Utils;
using ZKWeb.Plugins.Finance.Payment.Wechat.src.Components.PaymentApiHandlers;
using ZKWebStandard.Web;

namespace WxPayAPI {
	/// <summary>
	/// 微信支付设置
	/// </summary>
	public class WxPayConfig {
		/// <summary>
		/// 绑定支付的APPID（必须配置）
		/// 例如 wx2428e34e0e7dc6ef
		/// </summary>
		public string APPID { get; set; }
		/// <summary>
		/// 商户号（必须配置）
		/// 例如 1233410002
		/// </summary>
		public string MCHID { get; set; }
		/// <summary>
		/// 商户支付密钥，参考开户邮件设置（必须配置）
		/// 例如 e10adc3849ba56abbe56e056f20f883e
		/// </summary>
		public string KEY { get; set; }
		/// <summary>
		/// 公众帐号secert（仅JSAPI支付的时候需要配置）
		/// 例如 51c56b886b5be869567dd389b3e5d1d6
		/// </summary>
		public string APPSECRET { get; set; }
		/// <summary>
		/// 证书路径
		/// 应该是APP_Data下的路径
		/// 例如 cert/apiclient_cert.p12
		/// </summary>
		public string SSLCERT_PATH { get; set; }
		/// <summary>
		/// 证书密码
		/// 例如 1233410002
		/// </summary>
		public string SSLCERT_PASSWORD { get; set; }
		/// <summary>
		/// 支付结果通知回调url，用于商户接收支付结果
		/// 例如 http://paysdk.weixin.qq.com/example/ResultNotifyPage.aspx
		/// </summary>
		public string NOTIFY_URL { get; set; }
		/// <summary>
		/// APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP
		/// </summary>
		public string IP { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="apiData">接口数据</param>
		public WxPayConfig(WechatApiHandler.ApiData apiData) {
			APPID = apiData.AppId;
			MCHID = apiData.PartnerId;
			KEY = apiData.PartnerKey;
			APPSECRET = null;
			SSLCERT_PATH = null;
			SSLCERT_PASSWORD = null;
			NOTIFY_URL = PaymentUtils.GetReturnOrNotifyUrl(
				apiData.ReturnDomain, "/payment/wechat/notify");
			var ipAddress = NetworkUtils.GetClientIpAddress();

		}
	}
}