using System.Linq;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductSalesInfoDisplayFields {
	/// <summary>
	/// 重量
	/// </summary>
	[ExportMany]
	public class WeightDisplayField : IProductSalesInfoDisplayField {
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get { return "Weight"; } }

		/// <summary>
		/// 获取显示的Html
		/// </summary>
		public string GetDisplayHtml(IDatabaseContext context, Database.Product product) {
			// 获取最小和最大重量
			var weights = product.MatchedDatas.Where(d => d.Weight != null).Select(d => d.Weight.Value);
			var min = weights.Any() ? weights.Min() : 0;
			var max = weights.Any() ? weights.Max() : 0;
			// 无重量时不显示，最小和最大相同时显示{min}克，不相同时显示{min}~{max}克
			if (min == max && min == 0) {
				return null;
			} else if (min == max) {
				return string.Format(new T("{0:F2} gram"), min);
			}
			return string.Format(new T("{0:F2}~{1:F2} gram"), min, max);
		}
	}
}
