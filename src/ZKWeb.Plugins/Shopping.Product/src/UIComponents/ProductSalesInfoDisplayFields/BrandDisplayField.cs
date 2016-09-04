using System.Linq;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductSalesInfoDisplayFields.Interfaces;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductSalesInfoDisplayFields {
	using Product = Domain.Entities.Product;

	/// <summary>
	/// 品牌
	/// </summary>
	[ExportMany]
	public class BrandDisplayField : IProductSalesInfoDisplayField {
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get { return "Brand"; } }

		/// <summary>
		/// 获取显示的Html
		/// </summary>
		public string GetDisplayHtml(Product product) {
			// 获取名称中带品牌的属性并返回该值
			// 没有时返回null
			var value = product.FindPropertyValuesWhereNameContains(Name).FirstOrDefault();
			if (value != null) {
				return HttpUtils.HtmlEncode(new T(value.PropertyValueName));
			}
			return null;
		}
	}
}
