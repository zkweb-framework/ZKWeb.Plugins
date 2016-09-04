using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions {
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
		/// 获取价格组成部分的描述
		/// 例 "商品总价100 + 运费20 - 优惠50"
		/// </summary>
		/// <param name="parts">价格组成部分</param>
		/// <returns></returns>
		public static string GetDescription(this IList<OrderPriceCalcResult.Part> parts) {
			var builder = new StringBuilder();
			var isFirst = true;
			foreach (var part in parts) {
				if (isFirst) {
					isFirst = false;
					builder.Append(new T(part.Type));
					builder.Append(" ");
					builder.Append(part.Delta.ToString());
				} else {
					builder.Append((part.Delta >= 0) ? " + " : " - ");
					builder.Append(new T(part.Type));
					builder.Append(" ");
					builder.Append(Math.Abs(part.Delta).ToString());
				}
			}
			return builder.ToString();
		}
	}
}
