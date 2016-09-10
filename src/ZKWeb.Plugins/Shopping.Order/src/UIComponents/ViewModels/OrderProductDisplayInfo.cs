using DotLiquid;
using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels {
	/// <summary>
	/// 订单商品的显示信息
	/// 用于显示在购物车页和订单列表页等
	/// </summary>
	public class OrderProductDisplayInfo : ILiquidizable {
		/// <summary>
		/// 商品Id
		/// </summary>
		public Guid ProductId { get; set; }
		/// <summary>
		/// 订单商品Id，成员的值只有在显示"订单商品"时才会存在
		/// </summary>
		public Guid? OrderProductId { get; set; }
		/// <summary>
		/// 商品名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 商品主图缩略图的网页路径
		/// </summary>
		public string ImageWebPath { get; set; }
		/// <summary>
		/// 商品匹配参数
		/// </summary>
		public IDictionary<string, object> MatchedParameters { get; set; }
		/// <summary>
		/// 商品匹配参数的描述
		/// </summary>
		public string MatchedParametersDescription { get; set; }
		/// <summary>
		/// 单价
		/// </summary>
		public decimal UnitPrice { get; set; }
		/// <summary>
		/// 原始单价
		/// </summary>
		public decimal OriginalUnitPrice { get; set; }
		/// <summary>
		/// 货币
		/// </summary>
		public ICurrency Currency { get; set; }
		/// <summary>
		/// 单价字符串
		/// </summary>
		public string UnitPriceString { get; set; }
		/// <summary>
		/// 单价的描述
		/// </summary>
		public string UnitPriceDescription { get; set; }
		/// <summary>
		/// 原始单价字符串
		/// </summary>
		public string OriginalUnitPriceString { get; set; }
		/// <summary>
		/// 原始单价的描述
		/// </summary>
		public string OriginalUnitPriceDescription { get; set; }
		/// <summary>
		/// 数量
		/// </summary>
		public long Count { get; set; }
		/// <summary>
		/// 已发货数量
		/// </summary>
		public long ShippedCount { get; set; }
		/// <summary>
		/// 卖家Id
		/// </summary>
		public Guid? SellerId { get; set; }
		/// <summary>
		/// 卖家用户名
		/// </summary>
		public string Seller { get; set; }
		/// <summary>
		/// 商品状态
		/// </summary>
		public string State { get; set; }
		/// <summary>
		/// 商品类型
		/// </summary>
		public string Type { get; set; }
		/// <summary>
		/// 是否实体商品
		/// </summary>
		public bool IsRealProduct { get; set; }
		/// <summary>
		/// 附加数据
		/// </summary>
		public IDictionary<string, object> Extra { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public OrderProductDisplayInfo() {
			Extra = new Dictionary<string, object>();
		}

		/// <summary>
		/// 允许描画到模板
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return Hash.FromAnonymousObject(this);
		}
	}
}
