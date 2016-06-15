using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Shopping.Logistics.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Logistics.src.LogisticsTypes {
	/// <summary>
	/// 平邮
	/// </summary>
	[ExportMany]
	public class SurfaceMail : ILogisticsType {
		/// <summary>
		/// 平邮
		/// </summary>
		public string Type { get { return "SurfaceMail"; } }
	}
}
