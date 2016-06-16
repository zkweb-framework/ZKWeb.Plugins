using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Currency.src.Currencies {
	/// <summary>
	/// 美元
	/// </summary>
	[ExportMany]
	public class USD : ICurrency {
		public string Type { get { return "USD"; } }
		public string Prefix { get { return "$"; } }
		public string Suffix { get { return null; } }
	}
}
