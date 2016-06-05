using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Shopping.Order.src.Model;

namespace ZKWeb.Plugins.Shopping.Order.src.Extensions {
	/// <summary>
	/// 订单或订单商品的价格计算结果的扩展函数
	/// </summary>
	public static class OrderPriceCalcResultExtensions {
		/// <summary>
		/// 获取价格的组成部分的合计
		/// </summary>
		/// <param name="parts">价格组成部分</param>
		/// <returns></returns>
		public static decimal Sum(this IList<OrderPriceCalcResult.Part> parts) {
			return parts.Select(p => p.Delta).Sum();
		}

		/// <summary>
		/// 获取价格的组成部分的合计
		/// </summary>
		/// <param name="result">价格计算结果</param>
		/// <returns></returns>
		public static decimal Sum(this OrderPriceCalcResult result) {
			return result.Parts.Sum();
		}
	}
}
