using DryIocAttributes;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		/// 类目Id
		/// </summary>
		public virtual long CategoryId { get; set; }
		/// <summary>
		/// 属性Id
		/// </summary>
		public virtual long PropertyId { get; set; }
		/// <summary>
		/// 属性值Id，手工输入时等于null
		/// </summary>
		public virtual long? PropertyValueId { get; set; }
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
			Map(v => v.CategoryId);
			Map(v => v.PropertyId);
			Map(v => v.PropertyValueId);
			Map(v => v.PropertyValueName).Length(0xffff);
		}
	}
}
