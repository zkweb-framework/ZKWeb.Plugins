using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Currency.src.Currencies {
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
