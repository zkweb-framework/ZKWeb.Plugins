using DotLiquid;
using DryIocAttributes;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Product.src.Database {
	/// <summary>
	/// 商品属性值
	/// </summary>
	public class ProductPropertyValue : ILiquidizable {
		/// <summary>
		/// 属性值Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 属性值名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 所属的商品属性
		/// </summary>
		public virtual ProductProperty Property { get; set; }
		/// <summary>
		/// 显示顺序，从小到大
		/// 自动分配
		/// </summary>
		public virtual long DisplayOrder { get; set; }
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
		/// 支持描画到模板
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return new { Id, Name, Property };
		}

		/// <summary>
		/// 显示名称
		/// </summary>
		public override string ToString() {
			return Name;
		}
	}

	/// <summary>
	/// 商品属性值的数据库结构
	/// </summary>
	[ExportMany]
	public class ProductPropertyValueMap : ClassMap<ProductPropertyValue> {
		/// <summary>
		/// 初始化
		/// </summary>
		public ProductPropertyValueMap() {
			Id(p => p.Id);
			Map(p => p.Name).Length(0xffff);
			References(p => p.Property);
			Map(p => p.DisplayOrder);
			Map(p => p.CreateTime);
			Map(p => p.LastUpdated);
			Map(p => p.Remark).Length(0xffff);
		}
	}
}
