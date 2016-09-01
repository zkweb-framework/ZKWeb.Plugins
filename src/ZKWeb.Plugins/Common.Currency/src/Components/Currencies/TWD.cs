using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Currency.src.Components.Currencies {
	/// <summary>
	/// 新台币
	/// </summary>
	[ExportMany]
	public class TWD : ICurrency {
		public string Type { get { return "TWD"; } }
		public string Prefix { get { return "NT$"; } }
		public string Suffix { get { return null; } }
	}
}
