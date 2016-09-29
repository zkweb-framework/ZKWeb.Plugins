using ZKWeb.Plugins.Shopping.Product.src.Components.ProductMatchedDataMatchers.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;
using ZKWeb.Storage;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Components.ProductMatchedDataMatchers {
	/// <summary>
	/// 匹配购买数量
	/// </summary>
	[ExportMany]
	public class OrderCountMatcher : IProductMatchedDataMatcher {
		/// <summary>
		/// 判断是否匹配
		/// </summary>
		public bool IsMatched(ProductMatchParameters parameters, ProductMatchedData data) {
			// 获取订购数量的条件
			var orderCountGE = data.Conditions.GetOrderCountGE();
			if (orderCountGE == null || orderCountGE <= 1) {
				return true; // 没有指定条件
			}
			// 判断订购数量是否大于条件中指定的数量
			var orderCount = parameters.GetOrderCount();
			return orderCount >= orderCountGE.Value;
		}

		/// <summary>
		/// 获取使用Javascript判断是否匹配的函数
		/// </summary>
		/// <returns></returns>
		public string GetJavascriptMatchFunction() {
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			return fileStorage.GetResourceFile(
				"static", "shopping.product.js", "matched_data_matchers", "order_count.matcher.js").ReadAllText();
		}
	}
}
