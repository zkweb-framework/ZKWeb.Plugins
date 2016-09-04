using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Entities {
	/// <summary>
	/// 订单发货单包含的商品
	/// 多对多表，包含了发货件数
	/// </summary>
	[ExportMany]
	public class OrderDeliveryToOrderProduct :
		IEntity<Guid>, IEntityMappingProvider<OrderDeliveryToOrderProduct> {
		/// <summary>
		/// 数据Id
		/// 因为数据在编辑时会删除重建，其他表不能关联这里的Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 发货单
		/// </summary>
		public virtual OrderDelivery OrderDelivery { get; set; }
		/// <summary>
		/// 订单商品
		/// </summary>
		public virtual OrderProduct OrderProduct { get; set; }
		/// <summary>
		/// 发货件数
		/// </summary>
		public virtual long Count { get; set; }

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<OrderDeliveryToOrderProduct> builder) {
			builder.Id(p => p.Id);
			builder.References(p => p.OrderDelivery);
			builder.References(p => p.OrderProduct,
				new EntityMappingOptions() { Nullable = false });
			builder.Map(p => p.Count);
		}
	}
}
