using System.Collections.Generic;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Structs {
	/// <summary>
	/// 订单或订单商品的价格计算结果
	/// </summary>
	public class OrderPriceCalcResult {
		/// <summary>
		/// 价格的组成部分
		/// </summary>
		public IList<Part> Parts { get; set; }
		/// <summary>
		/// 货币单位
		/// </summary>
		public string Currency { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public OrderPriceCalcResult() {
			Parts = new List<Part>();
		}

		/// <summary>
		/// 价格的组成部分
		/// </summary>
		public class Part {
			/// <summary>
			/// 类型
			/// </summary>
			public string Type { get; set; }
			/// <summary>
			/// 影响量，可以是正数或负数
			/// </summary>
			public decimal Delta { get; set; }

			/// <summary>
			/// 初始化
			/// </summary>
			public Part() { }

			/// <summary>
			/// 初始化
			/// </summary>
			/// <param name="type">类型</param>
			/// <param name="delta">影响量</param>
			public Part(string type, decimal delta) {
				Type = type;
				Delta = delta.Normalize();
			}
		}
	}
}
