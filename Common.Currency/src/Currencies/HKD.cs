using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Currency.src.Currencies {
	/// <summary>
	/// 港元
	/// </summary>
	[ExportMany]
	public class HKD : ICurrency {
		public string Type { get { return "HKD"; } }
		public string Prefix { get { return "HK$"; } }
		public string Suffix { get { return null; } }
	}
}
