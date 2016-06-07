using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWeb.Utils.IocContainer;

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
