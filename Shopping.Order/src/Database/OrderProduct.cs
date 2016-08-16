using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.Model;

namespace ZKWeb.Plugins.Shopping.Order.src.Database {
	using ZKWeb.Database;
	using ZKWebStandard.Ioc;
	using Product = Product.src.Database.Product;

	/// <summary>
	/// 订单商品
	/// </summary>
	[ExportMany]
	public class OrderProduct : IEntity<long>, IEntityMappingProvider<OrderProduct> {
		/// <summary>
		/// 订单商品Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 所属的订单
		/// </summary>
		public virtual Order Order { get; set; }
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
		public virtual DateTime LastUpdated { get; set; }
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
			builder.References(p => p.Order, new EntityMappingOptions() {
				Nullable = false
			});
			builder.References(p => p.Product, new EntityMappingOptions() {
				Nullable = false
			});
			builder.Map(p => p.MatchParameters, new EntityMappingOptions() {
				WithSerialization = true
			});
			builder.Map(p => p.Count);
			builder.Map(p => p.UnitPrice);
			builder.Map(p => p.Currency, new EntityMappingOptions() {
				Nullable = false
			});
			builder.Map(p => p.UnitPriceCalcResult, new EntityMappingOptions() {
				WithSerialization = true
			});
			builder.Map(p => p.OriginalUnitPriceCalcResult, new EntityMappingOptions() {
				WithSerialization = true
			});
			builder.Map(p => p.CreateTime);
			builder.Map(p => p.LastUpdated);
			builder.HasMany(p => p.PropertyValues);
		}
	}
}
