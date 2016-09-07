using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Entities {
	using Product = Product.src.Domain.Entities.Product;

	/// <summary>
	/// 订单商品
	/// </summary>
	[ExportMany]
	public class OrderProduct :
		IHaveCreateTime, IHaveUpdateTime,
		IEntity<Guid>, IEntityMappingProvider<OrderProduct> {
		/// <summary>
		/// 订单商品Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 所属的订单
		/// </summary>
		public virtual SellerOrder Order { get; set; }
		/// <summary>
		/// 对应的商品
		/// </summary>
		public virtual Product Product { get; set; }
		/// <summary>
		/// 商品匹配参数
		/// 包含规格等信息，这里会包含购买数量但不应该使用这里的数量
		/// </summary>
		public virtual ProductMatchParameters MatchParameters { get; set; }
		/// <summary>
		/// 购买数量
		/// </summary>
		public virtual long Count { get; set; }
		/// <summary>
		/// 单价
		/// </summary>
		public virtual decimal UnitPrice { get; set; }
		/// <summary>
		/// 单价的货币
		/// </summary>
		public virtual string Currency { get; set; }
		/// <summary>
		/// 单价的计算结果
		/// </summary>
		public virtual OrderPriceCalcResult UnitPriceCalcResult { get; set; }
		/// <summary>
		/// 原始单价的计算结果
		/// 下单时生成且之后不会改变
		/// </summary>
		public virtual OrderPriceCalcResult OriginalUnitPriceCalcResult { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime UpdateTime { get; set; }
		/// <summary>
		/// 附加数据
		/// </summary>
		public virtual OrderProductItems Items { get; set; }
		/// <summary>
		/// 关联的属性值集合
		/// </summary>
		public virtual ISet<OrderProductToPropertyValue> PropertyValues { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public OrderProduct() {
			MatchParameters = new ProductMatchParameters();
			PropertyValues = new HashSet<OrderProductToPropertyValue>();
		}

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<OrderProduct> builder) {
			builder.Id(p => p.Id);
			builder.References(p => p.Order);
			builder.References(p => p.Product,
				new EntityMappingOptions() { Nullable = false });
			builder.Map(p => p.MatchParameters,
				new EntityMappingOptions() { WithSerialization = true });
			builder.Map(p => p.Count);
			builder.Map(p => p.UnitPrice);
			builder.Map(p => p.Currency,
				new EntityMappingOptions() { Nullable = false });
			builder.Map(p => p.UnitPriceCalcResult,
				new EntityMappingOptions() { WithSerialization = true });
			builder.Map(p => p.OriginalUnitPriceCalcResult,
				new EntityMappingOptions() { WithSerialization = true });
			builder.Map(p => p.CreateTime);
			builder.Map(p => p.UpdateTime);
			builder.Map(p => p.Items, new EntityMappingOptions() { WithSerialization = true });
			builder.HasMany(p => p.PropertyValues);
		}
	}
}
