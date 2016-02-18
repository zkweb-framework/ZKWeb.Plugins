using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Finance.Payment.Tenpay.src.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "TenpayPaymentApi", "财付通支付" },
			{ "Support pay transactions by tenpay", "支持通过财付通支付交易" },
			{ "Tenpay", "财付通" },
			{ "PartnerId", "商户Id" },
			{ "PartnerKey", "商户密钥" },
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
