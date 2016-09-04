using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Currency.src.Components.Currencies {
	/// <summary>
	/// 欧元
	/// </summary>
	[ExportMany]
	public class EUR : ICurrency {
		public string Type { get { return "EUR"; } }
		public string Prefix { get { return "€"; } }
		public string Suffix { get { return null; } }
	}
}
