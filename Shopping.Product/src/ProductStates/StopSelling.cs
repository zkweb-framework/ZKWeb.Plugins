using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductStates {
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
