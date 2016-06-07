using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Shopping.Logistics.src.Model;
using ZKWeb.Utils.IocContainer;

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
