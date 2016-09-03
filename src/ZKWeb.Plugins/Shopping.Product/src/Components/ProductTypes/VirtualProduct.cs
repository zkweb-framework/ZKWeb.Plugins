using ZKWeb.Plugins.Shopping.Product.src.Components.ProductTypes.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Components.ProductTypes {
	/// <summary>
	/// 虚拟商品
	/// </summary>
	[ExportMany]
	public class VirtualProduct : IAmVirtualProduct {
		/// <summary>
		/// 商品类型
		/// </summary>
		public string Type { get { return "VirtualProduct"; } }
	}
}
