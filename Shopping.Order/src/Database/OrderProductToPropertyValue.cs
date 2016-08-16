using ZKWeb.Database;
using ZKWeb.Plugins.Shopping.Product.src.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Database {
	/// <summary>
	/// 订单商品关联的属性值
	/// </summary>
	[ExportMany]
	public class OrderProductToPropertyValue :
		IEntity<long>, IEntityMappingProvider<OrderProductToPropertyValue> {
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
		/// 商品类目
		/// 从ProductToPropertyValue复制
		/// </summary>
		public virtual ProductCategory Category { get; set; }
		/// <summary>
		/// 商品属性
		/// 从ProductToPropertyValue复制
		/// </summary>
		public virtual ProductProperty Property { get; set; }
		/// <summary>
		/// 商品属性值
		/// 从ProductToPropertyValue复制
		/// </summary>
		public virtual ProductPropertyValue PropertyValue { get; set; }
		/// <summary>
		/// 属性值名称
		/// 从ProductToPropertyValue复制
		/// </summary>
		public virtual string PropertyValueName { get; set; }

		/// <summary>
		/// 配置的数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<OrderProductToPropertyValue> builder) {
			builder.Id(v => v.Id);
			builder.References(v => v.OrderProduct, new EntityMappingOptions() {
				Nullable = false
			});
			builder.References(v => v.Category);
			builder.References(v => v.Property);
			builder.References(v => v.PropertyValue);
			builder.Map(v => v.PropertyValueName);
		}
	}
}
