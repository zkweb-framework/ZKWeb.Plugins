using ZKWeb.Plugins.Shopping.Logistics.src.Components.LogisticsTypes.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Logistics.src.Components.LogisticsTypes {
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
