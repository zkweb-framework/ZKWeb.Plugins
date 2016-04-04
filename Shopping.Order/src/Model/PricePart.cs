using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Order.src.Model {
	/// <summary>
	/// 价格的组成部分
	/// </summary>
	public class PricePart {
		/// <summary>
		/// 类型
		/// </summary>
		public string Type { get; set; }
		/// <summary>
		/// 影响量，可以是正数或负数
		/// </summary>
		public decimal Delta { get; set; }
	}
}
