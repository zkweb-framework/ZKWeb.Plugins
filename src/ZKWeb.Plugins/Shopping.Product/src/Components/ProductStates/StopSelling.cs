using ZKWeb.Plugins.Shopping.Product.src.Components.ProductStates.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Components.ProductStates {
	/// <summary>
	/// 已下架
	/// </summary>
	[ExportMany]
	public class StopSelling : IProductState {
		/// <summary>
		/// 商品状态
		/// </summary>
		public string State { get { return "StopSelling"; } }
	}
}
