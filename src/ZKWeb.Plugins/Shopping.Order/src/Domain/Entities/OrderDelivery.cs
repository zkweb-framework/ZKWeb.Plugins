using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Entities {
	using Logistics = Logistics.src.Domain.Entities.Logistics;

	/// <summary>
	/// 订单发货单
	/// </summary>
	[ExportMany]
	public class OrderDelivery :
		IHaveCreateTime, IHaveUpdateTime,
		IEntity<Guid>, IEntityMappingProvider<OrderDelivery> {
		/// <summary>
		/// 发货单Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 发货单编号，唯一键
		/// </summary>
		public virtual string Serial { get; set; }
		/// <summary>
		/// 所属订单
		/// </summary>
		public virtual SellerOrder Order { get; set; }
		/// <summary>
		/// 使用的物流
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
		public virtual DateTime UpdateTime { get; set; }
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
			builder.References(d => d.Order);
			builder.References(d => d.Operator);
			builder.Map(d => d.CreateTime);
			builder.Map(d => d.UpdateTime);
			builder.Map(d => d.Remark);
			builder.HasMany(d => d.OrderProducts);
		}
	}
}
