using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.Currency.src.Currencies {
	/// <summary>
	/// 捷克克朗
	/// </summary>
	[ExportMany]
	public class CZK : ICurrency {
		public string Type { get { return "CZK"; } }
		public string Prefix { get { return "Kč"; } }
		public string Suffix { get { return null; } }
	}
}
