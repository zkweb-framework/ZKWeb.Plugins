using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Shopping.Logistics.src.Model;

namespace ZKWeb.Plugins.Shopping.Logistics.src.LogisticsTypes {
	/// <summary>
	/// 快递
	/// </summary>
	[ExportMany]
	public class Express : ILogisticsType {
		/// <summary>
		/// 物流类型
		/// </summary>
		public string Type { get { return "Express"; } }
	}
}
