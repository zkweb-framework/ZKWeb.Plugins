using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductSalesInfoDisplayFields.Interfaces;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductSalesInfoDisplayFields {
	using Product = Domain.Entities.Product;

	/// <summary>
	/// 卖家
	/// </summary>
	[ExportMany]
	public class SellerDisplayField : IProductSalesInfoDisplayField {
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get { return "Seller"; } }

		/// <summary>
		/// 获取显示的Html
		/// </summary>
		public string GetDisplayHtml(Product product) {
			var seller = product.Seller;
			return seller == null ? null : HttpUtils.HtmlEncode(seller.Username);
		}
	}
}
