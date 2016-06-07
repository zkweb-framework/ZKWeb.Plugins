using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Shopping.Order.src.Database {
	/// <summary>
	/// 订单发货单包含的商品
	/// </summary>
	public class OrderDeliveryToOrderProduct {
		/// <summary>
		/// 数据Id
		/// 因为数据在编辑时会删除重建，其他表不能关联这里的Id
		/// </summary>
		public virtual long Id { get; set; }
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
	}

	/// <summary>
	/// 订单发货单包含的商品的数据库结构
	/// </summary>
	[ExportMany]
	public class OrderDeliveryToOrderProductMap : ClassMap<OrderDeliveryToOrderProduct> {
		/// <summary>
		/// 初始化
		/// </summary>
		public OrderDeliveryToOrderProductMap() {
			Id(p => p.Id);
			References(p => p.OrderDelivery).Not.Nullable();
			References(p => p.OrderProduct).Not.Nullable();
			Map(p => p.Count);
		}
	}
}
