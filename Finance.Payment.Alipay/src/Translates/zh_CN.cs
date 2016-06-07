using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Finance.Payment.Alipay.src.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "AlipayPaymentApi", "支付宝支付" },
			{ "Support pay transactions by alipay", "支持通过支付宝支付交易" },
			{ "Alipay", "支付宝" },
			{ "PartnerId", "商户Id" },
			{ "PartnerEmail", "商户邮箱" },
			{ "PartnerKey", "商户密钥" },
			{ "ServiceType", "服务类型" },
			{ "ImmediateArrival", "即时到账" },
			{ "SecuredTransaction", "担保交易" },
			{ "DualInterface", "双接口" },
			{ "ReturnDomain", "返回域名" },
			{ "keep empty will use the default domain", "留空时使用默认域名" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
