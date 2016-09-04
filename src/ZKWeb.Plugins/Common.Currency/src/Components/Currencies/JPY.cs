using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Currency.src.Components.Currencies {
	/// <summary>
	/// 日元
	/// </summary>
	[ExportMany]
	public class JPY : ICurrency {
		public string Type { get { return "JPY"; } }
		public string Prefix { get { return "JP¥"; } }
		public string Suffix { get { return null; } }
	}
}
