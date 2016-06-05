using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Order.src.Model {
	/// <summary>
	/// 订单商品的显示信息
	/// 用于显示在购物车页和订单列表页等
	/// </summary>
	public class OrderProductDisplayInfo {
		/// <summary>
		/// 商品Id
		/// </summary>
		public long ProductId { get; set; }
		/// <summary>
		/// 订单商品Id，成员的值只有在显示"订单商品"时才会存在
		/// </summary>
		public long OrderProductId { get; set; }
		/// <summary>
		/// 商品名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 商品主图缩略图的网页路径
		/// </summary>
		public string ImageWebPath { get; set; }
		/// <summary>
		/// 商品匹配参数的描述
		/// </summary>
		public string MatchedParametersDescription { get; set; }
		/// <summary>
		/// 价格
		/// </summary>
		public decimal Price { get; set; }
		/// <summary>
		/// 原始价格
		/// </summary>
		public decimal OriginalPrice { get; set; }
		/// <summary>
		/// 货币
		/// </summary>
		public string Currency { get; set; }
		/// <summary>
		/// 价格的描述
		/// </summary>
		public string PriceDescription { get; set; }
		/// <summary>
		/// 原始价格描述
		/// </summary>
		public string OriginalPriceDescription { get; set; }
		/// <summary>
		/// 数量
		/// </summary>
		public long Count { get; set; }
	}
}
