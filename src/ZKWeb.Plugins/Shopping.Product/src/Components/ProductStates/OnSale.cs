using ZKWeb.Plugins.Shopping.Product.src.Components.ProductStates.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Components.ProductStates {
	/// <summary>
	/// 上架中
	/// </summary>
	[ExportMany]
	public class OnSale : IAmVisibleToThePublic, IAmPurchasable {
		/// <summary>
		/// 商品状态
		/// </summary>
		public string State { get { return "OnSale"; } }
	}
}
