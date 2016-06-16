using FluentNHibernate.Mapping;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Database {
	/// <summary>
	/// 订单商品关联的属性值
	/// </summary>
	public class OrderProductToPropertyValue {
		/// <summary>
		/// 数据Id
		/// 因为数据在编辑时会删除重建，其他表不能关联这里的Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 订单商品
		/// </summary>
		public virtual OrderProduct OrderProduct { get; set; }
		/// <summary>
		/// 类目Id
		/// 从ProductToPropertyValue复制
		/// </summary>
		public virtual long CategoryId { get; set; }
		/// <summary>
		/// 属性Id
		/// 从ProductToPropertyValue复制
		/// </summary>
		public virtual long PropertyId { get; set; }
		/// <summary>
		/// 属性值Id
		/// 从ProductToPropertyValue复制
		/// </summary>
		public virtual long? PropertyValueId { get; set; }
		/// <summary>
		/// 属性值名称
		/// 从ProductToPropertyValue复制
		/// </summary>
		public virtual string PropertyValueName { get; set; }
	}

	/// <summary>
	/// 订单商品关联的属性值的数据库结构
	/// </summary>
	[ExportMany]
	public class OrderProductToPropertyValueMap : ClassMap<OrderProductToPropertyValue> {
		/// <summary>
		/// 初始化
		/// </summary>
		public OrderProductToPropertyValueMap() {
			Id(v => v.Id);
			References(v => v.OrderProduct).Not.Nullable();
			Map(v => v.CategoryId).Index("Idx_CategoryId");
			Map(v => v.PropertyId).Index("Idx_PropertyId");
			Map(v => v.PropertyValueId).Index("Idx_PropertyValueId");
			Map(v => v.PropertyValueName);
		}
	}
}
