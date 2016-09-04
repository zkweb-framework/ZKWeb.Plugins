using ZKWeb.Plugins.Shopping.Product.src.Components.ProductStates.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Components.ProductStates {
	/// <summary>
	/// 准备上架
	/// </summary>
	[ExportMany]
	public class WaitForSales : IAmVisibleToThePublic {
		/// <summary>
		/// 商品状态
		/// </summary>
		public string State { get { return "WaitForSales"; } }
	}
}
