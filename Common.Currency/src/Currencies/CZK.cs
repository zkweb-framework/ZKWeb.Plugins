using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Currency.src.Currencies {
	/// <summary>
	/// 捷克克朗
	/// </summary>
	[ExportMany]
	public class CZK : ICurrency {
		public string Type { get { return "CZK"; } }
		public string Prefix { get { return "Kč"; } }
		public string Suffix { get { return null; } }
	}
}
