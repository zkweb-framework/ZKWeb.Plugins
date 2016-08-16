using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Database {
	using ZKWeb.Database;
	using Logistics = Logistics.src.Database.Logistics;

	/// <summary>
	/// 订单发货单
	/// </summary>
	[ExportMany]
	public class OrderDelivery : IEntity<long>, IEntityMappingProvider<OrderDelivery> {
		/// <summary>
		/// 发货单Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 发货单编号，唯一键
		/// </summary>
		public virtual string Serial { get; set; }
		/// <summary>
		/// 订单
		/// </summary>
		public virtual Order Order { get; set; }
		/// <summary>
		/// 物流
		/// </summary>
		public virtual Logistics Logistics { get; set; }
		/// <summary>
		/// 物流给出的发货编号（快递单编号）
		/// 虚拟发货时不需要
		/// </summary>
		public virtual string LogisticsSerial { get; set; }
		/// <summary>
		/// 发货人
		/// 允许但一般不等于null
		/// </summary>
		public virtual User Operator { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime LastUpdated { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public virtual string Remark { get; set; }
		/// <summary>
		/// 包含的商品集合
		/// </summary>
		public virtual ISet<OrderDeliveryToOrderProduct> OrderProducts { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public OrderDelivery() {
			OrderProducts = new HashSet<OrderDeliveryToOrderProduct>();
		}

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<OrderDelivery> builder) {
			builder.Id(d => d.Id);
			builder.Map(d => d.Serial, new EntityMappingOptions() {
				Nullable = false, Unique = true, Length = 255
			});
			builder.References(d => d.Order, new EntityMappingOptions() {
				Nullable = false
			});
			builder.References(d => d.Operator);
			builder.Map(d => d.CreateTime);
			builder.Map(d => d.LastUpdated);
			builder.Map(d => d.Remark);
			builder.HasMany(d => d.OrderProducts);
		}
	}
}
