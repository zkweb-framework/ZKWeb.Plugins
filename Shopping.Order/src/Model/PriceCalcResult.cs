using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Order.src.Model {
	/// <summary>
	/// 价格计算结果
	/// </summary>
	public class PriceCalcResult {
		/// <summary>
		/// 价格的组成部分
		/// </summary>
		public List<PricePart> Parts { get; set; }
		/// <summary>
		/// 货币单位
		/// </summary>
		public string Currency { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public PriceCalcResult() {
			Parts = new List<PricePart>();
		}
	}
}
