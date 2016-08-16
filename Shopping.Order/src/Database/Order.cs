using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Database {
	/// <summary>
	/// 订单
	/// </summary>
	[ExportMany]
	public class Order : IEntity<long>, IEntityMappingProvider<Order> {
		/// <summary>
		/// 订单Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 订单编号，唯一键
		/// </summary>
		public virtual string Serial { get; set; }
		/// <summary>
		/// 买家用户，没有时等于null
		/// </summary>
		public virtual User Buyer { get; set; }
		/// <summary>
		/// 买家会话，已有买家用户时等于null
		/// 这里不关联会话对象避免删除问题
		/// </summary>
		public virtual string BuyerSessionId { get; set; }
		/// <summary>
		/// 卖家用户，没有时等于null
		/// </summary>
		public virtual User Seller { get; set; }
		/// <summary>
		/// 订单状态
		/// </summary>
		public virtual OrderState State { get; set; }
		/// <summary>
		/// 订单参数
		/// 包含收货地址，选择的物流Id和收款接口Id等
		/// </summary>
		public virtual OrderParameters OrderParameters { get; set; }
		/// <summary>
		/// 当前的订单总金额
		/// </summary>
		public virtual decimal TotalCost { get; set; }
		/// <summary>
		/// 货币单位
		/// </summary>
		public virtual string Currency { get; set; }
		/// <summary>
		/// 当前的订单总金额的计算结果
		/// </summary>
		public virtual OrderPriceCalcResult TotalCostCalcResult { get; set; }
		/// <summary>
		/// 原始的订单总金额的计算结果
		/// 下单时生成且之后不会改变
		/// </summary>
		public virtual OrderPriceCalcResult OriginalTotalCostCalcResult { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime LastUpdated { get; set; }
		/// <summary>
		/// 各个状态的切换时间
		/// </summary>
		public virtual OrderStateTimes StateTimes { get; set; }
		/// <summary>
		/// 买家备注
		/// </summary>
		public virtual string BuyerRemark { get; set; }
		/// <summary>
		/// 卖家备注
		/// </summary>
		public virtual string SellerRemark { get; set; }
		/// <summary>
		/// 订单商品的集合
		/// </summary>
		public virtual ISet<OrderProduct> OrderProducts { get; set; }
		/// <summary>
		/// 订单留言的集合
		/// </summary>
		public virtual ISet<OrderComment> OrderComments { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public Order() {
			OrderParameters = new OrderParameters();
			StateTimes = new OrderStateTimes();
			OrderProducts = new HashSet<OrderProduct>();
			OrderComments = new HashSet<OrderComment>();
		}

		/// <summary>
		/// 显示名称
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Serial;
		}

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<Order> builder) {
			builder.Id(o => o.Id);
			builder.Map(o => o.Serial, new EntityMappingOptions() {
				Nullable = false, Unique = true, Length = 255
			});
			builder.References(o => o.Buyer);
			builder.Map(o => o.BuyerSessionId, new EntityMappingOptions() {
				Column = "BuyerSessionId_",
				Index = "Idx_BuyerSessionId_"
			});
			builder.References(o => o.Seller);
			builder.Map(o => o.State);
			builder.Map(o => o.OrderParameters, new EntityMappingOptions() {
				WithSerialization = true
			});
			builder.Map(o => o.TotalCost);
			builder.Map(o => o.Currency, new EntityMappingOptions() {
				Nullable = false
			});
			builder.Map(o => o.TotalCostCalcResult, new EntityMappingOptions() {
				WithSerialization = true
			});
			builder.Map(o => o.OriginalTotalCostCalcResult, new EntityMappingOptions() {
				WithSerialization = true
			});
			builder.Map(o => o.CreateTime);
			builder.Map(o => o.LastUpdated);
			builder.Map(o => o.StateTimes, new EntityMappingOptions() {
				WithSerialization = true
			});
			builder.Map(o => o.BuyerRemark);
			builder.Map(o => o.SellerRemark);
			builder.HasMany(o => o.OrderProducts);
			builder.HasMany(o => o.OrderComments);
		}
	}
}
