using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Currency.src.Model;

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
