using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Currency.src.Components.Currencies {
	/// <summary>
	/// 韩元
	/// </summary>
	[ExportMany]
	public class KRW : ICurrency {
		public string Type { get { return "KRW"; } }
		public string Prefix { get { return "₩"; } }
		public string Suffix { get { return null; } }
	}
}
