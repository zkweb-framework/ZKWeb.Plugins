using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.Alipay.src.Components.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			// TODO: 翻译到其他语言
			{ "AlipayPaymentApi", "支付宝支付" },
			{ "Support pay transactions by alipay", "支持通过支付宝支付交易" },
			{ "Alipay", "支付宝" },
			{ "PartnerId", "商户Id" },
			{ "PartnerId, usually starts with 2088", "商品Id, 一般以2088开始" },
			{ "PayeePartnerId", "收款商户Id" },
			{ "PayeePartnerId, usually same with PartnerId", "收款商户Id, 一般和商户Id相同" },
			{ "PartnerKey", "商户密钥" },
			{ "PartnerKey (RSA with SHA1), usually starts with -----BEGIN RSA PRIVATE KEY-----",
				"商户密钥 (RSA + SHA1), 一般以-----BEGIN RSA PRIVATE KEY-----开始" },
			{ "ServiceType", "服务类型" },
			{ "ImmediateArrival", "即时到账" },
			{ "SecuredTransaction", "担保交易" },
			{ "DualInterface", "双接口" },
			{ "WapPay", "手机支付" },
			{ "ReturnDomain", "返回域名" },
			{ "keep empty will use the default domain", "留空时使用默认域名" },
			{ "Alipay only support CNY", "支付宝只支持人民币支付" },
			{ "Alipay is unsupported on .net core yet", "支付宝目前不支持在.Net Core上使用" },
			{ "Call alipay send goods api success", "调用支付宝发货接口成功" },
			{ "Call alipay send goods api failed: {0}", "调用支付宝发货接口失败: {1}" },
			{ "transaction with serial {0} not exist", "序号为{0}的交易不存在" },
			{ "Transaction amount not matched, excepted '{0}' but actual is '{1}'",
				"交易金额不匹配, 预计支付金额是'{0}'但是实际支付金额是'{1}'" },
			{ "check alipay sign failed", "检查支付宝签名失败" },
			{ "Unknown alipay trade status '{0}'", "不支持的支付宝交易状态: '{0}'" },
			{ "Unknown alipay refund status: '{0}'", "不支持的支付宝退款状态: '{0}'" },
			{ "Buyer closed transaction on alipay", "买家在支付宝上关闭了交易" },
			{ "Buyer require refund on alipay", "买家在支付宝上申请了退款" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
