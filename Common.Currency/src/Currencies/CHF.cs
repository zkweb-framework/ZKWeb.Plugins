using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Currency.src.Currencies {
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
