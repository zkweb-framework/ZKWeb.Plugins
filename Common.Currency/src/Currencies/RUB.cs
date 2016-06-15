using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Currency.src.Currencies {
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
