using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Entities.Bases {
	/// <summary>
	/// 订单的基础类
	/// </summary>
	public abstract class OrderBase<TOrder> :
		IHaveCreateTime, IHaveUpdateTime, IHaveDeleted,
		IEntity<Guid>, IEntityMappingProvider<TOrder>
		where TOrder : OrderBase<TOrder> {
		/// <summary>
		/// 订单Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 订单编号，唯一键
		/// </summary>
		public virtual string Serial { get; set; }
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
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<TOrder> builder) {
			builder.Id(o => o.Id);
			builder.Map(o => o.Serial, new EntityMappingOptions() {
				Nullable = false, Unique = true, Length = 255
			});
			builder.Map(o => o.Owner);
			builder.Map(o => o.CreateTime);
			builder.Map(o => o.UpdateTime);
			builder.Map(o => o.Deleted);
			builder.Map(o => o.Remark);
		}

		/// <summary>
		/// 显示名称
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Serial;
		}
	}
}
