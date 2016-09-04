using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities.Bases;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Entities {
	/// <summary>
	/// 卖家订单
	/// 保存订单的参数和状态
	/// 不和买家订单关联避免循环依赖
	/// </summary>
	[ExportMany]
	public class SellerOrder : OrderBase<SellerOrder> {
		/// <summary>
		/// 订单编号，唯一键
		/// </summary>
		public virtual string Serial { get; set; }
		/// <summary>
		/// 买家用户
		/// </summary>
		public virtual User Buyer { get; set; }
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
		/// 各个状态的切换时间
		/// </summary>
		public virtual OrderStateTimes StateTimes { get; set; }
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
		public SellerOrder() {
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
		public override void Configure(IEntityMappingBuilder<SellerOrder> builder) {
			base.Configure(builder);
			builder.Map(o => o.Serial, new EntityMappingOptions() {
				Nullable = false, Unique = true, Length = 255
			});
			builder.References(o => o.Buyer);
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
			builder.Map(o => o.StateTimes, new EntityMappingOptions() {
				WithSerialization = true
			});
			builder.HasMany(o => o.OrderProducts);
			builder.HasMany(o => o.OrderComments);
		}
	}
}
