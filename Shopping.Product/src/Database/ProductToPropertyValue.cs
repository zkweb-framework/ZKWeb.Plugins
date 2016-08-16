using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Database {
	/// <summary>
	/// 商品使用的属性值
	/// </summary>
	[ExportMany]
	public class ProductToPropertyValue :
		IEntity<long>, IEntityMappingProvider<ProductToPropertyValue> {
		/// <summary>
		/// 数据Id
		/// 因为数据在编辑时会删除重建，其他表不能关联这里的Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 属于的商品
		/// </summary>
		public virtual Product Product { get; set; }
		/// <summary>
		/// 属性
		/// </summary>
		public virtual ProductProperty Property { get; set; }
		/// <summary>
		/// 属性值，手工输入时等于null
		/// </summary>
		public virtual ProductPropertyValue PropertyValue { get; set; }
		/// <summary>
		/// 属性值名称，允许手动修改
		/// </summary>
		public virtual string PropertyValueName { get; set; }

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<ProductToPropertyValue> builder) {
			builder.Id(v => v.Id);
			builder.References(v => v.Product, new EntityMappingOptions() { Nullable = false });
			builder.References(v => v.Property, new EntityMappingOptions() { Nullable = false });
			builder.References(v => v.PropertyValue);
			builder.Map(v => v.PropertyValueName);
		}
	}
}
