using FluentNHibernate.Mapping;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Database {
	/// <summary>
	/// 商品使用的属性值
	/// </summary>
	public class ProductToPropertyValue {
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
	}

	/// <summary>
	/// 商品使用的属性值的数据库结构
	/// </summary>
	[ExportMany]
	public class ProductToPropertyValueMap : ClassMap<ProductToPropertyValue> {
		/// <summary>
		/// 初始化
		/// </summary>
		public ProductToPropertyValueMap() {
			Id(v => v.Id);
			References(v => v.Product);
			References(v => v.Property).Not.Nullable();
			References(v => v.PropertyValue);
			Map(v => v.PropertyValueName).Length(0xffff);
		}
	}
}
