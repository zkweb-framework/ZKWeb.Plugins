using ZKWeb.Plugins.Shopping.Logistics.src.Components.LogisticsTypes.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Logistics.src.Components.LogisticsTypes {
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
