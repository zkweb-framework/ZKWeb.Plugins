using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Database;
using ZKWeb.Plugins.Shopping.Product.src.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductSalesInfoDisplayFields {
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
		public string GetDisplayHtml(DatabaseContext context, Database.Product product) {
			var seller = product.Seller;
			return seller == null ? null : HttpUtility.HtmlEncode(seller.Username);
		}
	}
}
