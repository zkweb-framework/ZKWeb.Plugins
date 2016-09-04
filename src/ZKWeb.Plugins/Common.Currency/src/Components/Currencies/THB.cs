using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Currency.src.Components.Currencies {
	/// <summary>
	/// 泰铢
	/// </summary>
	[ExportMany]
	public class THB : ICurrency {
		public string Type { get { return "THB"; } }
		public string Prefix { get { return "฿"; } }
		public string Suffix { get { return null; } }
	}
}
