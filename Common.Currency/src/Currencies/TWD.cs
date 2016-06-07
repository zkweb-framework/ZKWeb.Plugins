using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.Currency.src.Currencies {
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
