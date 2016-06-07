using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.Currency.src.Currencies {
	/// <summary>
	/// 人民币
	/// </summary>
	[ExportMany]
	public class CNY : ICurrency {
		public string Type { get { return "CNY"; } }
		public string Prefix { get { return "¥"; } }
		public string Suffix { get { return null; } }
	}
}
