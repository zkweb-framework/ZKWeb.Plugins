using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Currency.src.Components.Currencies {
	/// <summary>
	/// 卢布
	/// </summary>
	[ExportMany]
	public class RUB : ICurrency {
		public string Type { get { return "RUB"; } }
		public string Prefix { get { return "₽"; } }
		public string Suffix { get { return null; } }
	}
}
