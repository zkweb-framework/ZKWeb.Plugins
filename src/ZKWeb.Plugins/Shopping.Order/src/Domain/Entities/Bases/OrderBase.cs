using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Entities.Bases {
	/// <summary>
	/// 订单的基础类
	/// </summary>
	public abstract class OrderBase<TOrder> :
		IHaveCreateTime, IHaveUpdateTime, IHaveDeleted, IHaveOwner,
		IEntity<Guid>, IEntityMappingProvider<TOrder>
		where TOrder : OrderBase<TOrder> {
		/// <summary>
		/// 订单Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 所属人，买家或卖家，没有时等于null
		/// </summary>
		public virtual User Owner { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime UpdateTime { get; set; }
		/// <summary>
		/// 是否已删除
		/// </summary>
		public virtual bool Deleted { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public virtual string Remark { get; set; }
		/// <summary>
		/// 备注旗帜
		/// </summary>
		public virtual OrderRemarkFlags RemarkFlags { get; set; }
		/// <summary>
		/// 附加数据
		/// </summary>
		public virtual OrderItems Items { get; set; }

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<TOrder> builder) {
			var typeName = typeof(TOrder).Name;
			builder.Id(o => o.Id);
			builder.References(o => o.Owner);
			builder.Map(o => o.CreateTime, new EntityMappingOptions() { Index = $"Idx_{typeName}_CreateTime" });
			builder.Map(o => o.UpdateTime, new EntityMappingOptions() { Index = $"Idx_{typeName}_UpdateTime" });
			builder.Map(o => o.Deleted);
			builder.Map(o => o.Remark);
			builder.Map(o => o.RemarkFlags, new EntityMappingOptions() { Index = $"Idx_{typeName}_RemarkFlags" });
			builder.Map(o => o.Items, new EntityMappingOptions() { WithSerialization = true });
		}
	}
}
