using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductStates {
	/// <summary>
	/// 上架中
	/// </summary>
	[ExportMany]
	public class OnSale : IProductState {
		/// <summary>
		/// 商品状态
		/// </summary>
		public string State { get { return "OnSale"; } }
	}
}
