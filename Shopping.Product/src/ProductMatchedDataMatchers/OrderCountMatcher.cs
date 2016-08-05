using System.Collections.Generic;
using System.IO;
using ZKWeb.Plugins.Shopping.Product.src.Database;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Server;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductMatchedDataMatchers {
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
			var pathManager = Application.Ioc.Resolve<PathManager>();
			return File.ReadAllText(pathManager.GetResourceFullPath(
				"static", "shopping.product.js", "matched_data_matchers", "order_count.matcher.js"));
		}
	}
}
