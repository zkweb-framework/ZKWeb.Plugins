using ZKWeb.Plugins.Shopping.Product.src.Components.ProductTypes.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Components.ProductTypes {
	/// <summary>
	/// 实体商品
	/// </summary>
	[ExportMany]
	public class RealProduct : IAmRealProduct {
		/// <summary>
		/// 商品类型
		/// </summary>
		public string Type { get { return "RealProduct"; } }
	}
}
