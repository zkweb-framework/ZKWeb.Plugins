using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Currency.src.Components.Currencies {
	/// <summary>
	/// 瑞士法郎
	/// </summary>
	[ExportMany]
	public class CHF : ICurrency {
		public string Type { get { return "CHF"; } }
		public string Prefix { get { return "CHF"; } }
		public string Suffix { get { return null; } }
	}
}
