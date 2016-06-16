using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Shopping.Product.src.Config {
	/// <summary>
	/// 商品列表设置
	/// </summary>
	[GenericConfig("Shopping.Product.ProductListSettings", CacheTime = 15)]
	public class ProductListSettings {
		/// <summary>
		/// 每页显示的商品数量
		/// </summary>
		public int ProductsPerPage { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductListSettings() {
			ProductsPerPage = 24;
		}
	}
}
