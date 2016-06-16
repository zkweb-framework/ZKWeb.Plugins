using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Database {
	using Logistics = Logistics.src.Database.Logistics;

	/// <summary>
	/// 订单发货单
	/// </summary>
	public class OrderDelivery {
		/// <summary>
		/// 发货单Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 发货单编号
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
	}

	/// <summary>
	/// 订单发货单的数据库结构
	/// </summary>
	[ExportMany]
	public class OrderDeliveryMap : ClassMap<OrderDelivery> {
		/// <summary>
		/// 初始化
		/// </summary>
		public OrderDeliveryMap() {
			Id(d => d.Id);
			Map(d => d.Serial).Not.Nullable().Unique();
			References(d => d.Order).Not.Nullable();
			References(d => d.Operator);
			Map(d => d.CreateTime);
			Map(d => d.LastUpdated);
			Map(d => d.Remark).Length(0xffff);
			HasMany(d => d.OrderProducts).Cascade.AllDeleteOrphan();
		}
	}
}
