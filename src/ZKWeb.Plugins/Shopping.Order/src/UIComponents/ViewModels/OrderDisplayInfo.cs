using DotLiquid;
using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels {
	/// <summary>
	/// 订单的显示信息
	/// 用于显示在订单详情页和订单列表页等
	/// </summary>
	public class OrderDisplayInfo : ILiquidizable {
		/// <summary>
		/// 订单Id
		/// </summary>
		public Guid Id { get; set; }
		/// <summary>
		/// 流水号
		/// </summary>
		public string Serial { get; set; }
		/// <summary>
		/// 买家Id
		/// </summary>
		public Guid? BuyerId { get; set; }
		/// <summary>
		/// 买家用户名
		/// </summary>
		public string Buyer { get; set; }
		/// <summary>
		/// 卖家Id
		/// </summary>
		public Guid? SellerId { get; set; }
		/// <summary>
		/// 卖家用户名
		/// </summary>
		public string Seller { get; set; }
		/// <summary>
		/// 订单状态
		/// </summary>
		public string State { get; set; }
		/// <summary>
		/// 订单状态的描述
		/// </summary>
		public string StateDescription { get; set; }
		/// <summary>
		/// 订单状态到各个状态的时间
		/// </summary>
		public IDictionary<string, string> StateTimes { get; set; }
		/// <summary>
		/// 订单参数
		/// </summary>
		public OrderParameters OrderParameters { get; set; }
		/// <summary>
		/// 订单总金额
		/// </summary>
		public decimal TotalCost { get; set; }
		/// <summary>
		/// 订单总金额的字符串，经过货币格式化
		/// </summary>
		public string TotalCostString { get; set; }
		/// <summary>
		/// 订单总金额的描述
		/// </summary>
		public string TotalCostDescription { get; set; }
		/// <summary>
		/// 订单总金额的计算结果对象
		/// </summary>
		public OrderPriceCalcResult TotalCostCalcResult { get; set; }
		/// <summary>
		/// 原始订单总金额
		/// </summary>
		public decimal OriginalTotalCost { get; set; }
		/// <summary>
		/// 原始订单总金额的字符串，经过货币格式化
		/// </summary>
		public string OriginalTotalCostString { get; set; }
		/// <summary>
		/// 原始订单总金额的描述
		/// </summary>
		public string OriginalTotalCostDescription { get; set; }
		/// <summary>
		/// 原始订单总金额的计算结果对象
		/// </summary>
		public OrderPriceCalcResult OriginalTotalCostResult { get; set; }
		/// <summary>
		/// 货币单位
		/// </summary>
		public ICurrency Currency { get; set; }
		/// <summary>
		/// 备注旗帜
		/// </summary>
		public string RemarkFlags { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public string CreateTime { get; set; }
		/// <summary>
		/// 最后一条留言
		/// </summary>
		public string LastComment { get; set; }
		/// <summary>
		/// 查看交易的Url格式
		/// </summary>
		public string ViewTransactionUrlFormat { get; set; }
		/// <summary>
		/// 查看发货单的Url格式
		/// </summary>
		public string ViewDeliveryUrlFormat { get; set; }
		/// <summary>
		/// 警告信息列表
		/// </summary>
		public IList<HtmlString> WarningHtmls { get; set; }
		/// <summary>
		/// 订单详情的工具栏按钮列表
		/// </summary>
		public IList<HtmlString> ToolButtonHtmls { get; set; }
		/// <summary>
		/// 订单详情的内容列表
		/// </summary>
		public IList<HtmlString> SubjectHtmls { get; set; }
		/// <summary>
		/// 操作列表
		/// </summary>
		public IList<HtmlString> ActionHtmls { get; set; }
		/// <summary>
		/// 订单商品的显示信息列表
		/// </summary>
		public IList<OrderProductDisplayInfo> OrderProducts { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public OrderDisplayInfo() {
			StateTimes = new Dictionary<string, string>();
			WarningHtmls = new List<HtmlString>();
			ToolButtonHtmls = new List<HtmlString>();
			SubjectHtmls = new List<HtmlString>();
			ActionHtmls = new List<HtmlString>();
			OrderProducts = new List<OrderProductDisplayInfo>();
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
