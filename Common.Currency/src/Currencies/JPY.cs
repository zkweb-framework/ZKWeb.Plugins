using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Currency.src.Model;

namespace ZKWeb.Plugins.Common.Currency.src.Currencies {
	/// <summary>
	/// 日元
	/// </summary>
	[ExportMany]
	public class JPY : ICurrency {
		public string Type { get { return "JPY"; } }
		public string Prefix { get { return "JP¥"; } }
		public string Suffix { get { return null; } }
	}
}
