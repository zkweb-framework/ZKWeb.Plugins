using DotLiquid;
using DryIocAttributes;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Shopping.Product.src.Model;

namespace ZKWeb.Plugins.Shopping.Product.src.Database {
	/// <summary>
	/// 商品属性
	/// </summary>
	public class ProductProperty : ILiquidizable {
		/// <summary>
		/// 属性Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 属性名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 是否销售属性
		/// 销售属性时买家在购买时必须选择该属性的值
		/// </summary>
		public virtual bool IsSalesProperty { get; set; }
		/// <summary>
		/// 输入控件类型
		/// </summary>
		public virtual ProductPropertyControlType ControlType { get; set; }
		/// <summary>
		/// 属性值列表，一对多
		/// </summary>
		public virtual ISet<ProductPropertyValue> PropertyValues { get; set; }
		/// <summary>
		/// 显示顺序，从小到大
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
		/// 是否已删除
		/// </summary>
		public virtual bool Deleted { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public virtual string Remark { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductProperty() {
			ControlType = ProductPropertyControlType.TextBox;
			PropertyValues = new HashSet<ProductPropertyValue>();
			DisplayOrder = 10000;
		}

		/// <summary>
		/// 支持描画到模板
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return new {
				Id, Name,
				IsSalesProperty, ControlType, PropertyValues
			};
		}

		/// <summary>
		/// 显示名称
		/// </summary>
		public override string ToString() {
			return Name;
		}
	}

	/// <summary>
	/// 商品属性的数据库结构
	/// </summary>
	[ExportMany]
	public class ProductPropertyMap : ClassMap<ProductProperty> {
		/// <summary>
		/// 初始化
		/// </summary>
		public ProductPropertyMap() {
			Id(p => p.Id);
			Map(p => p.Name);
			Map(p => p.IsSalesProperty);
			Map(p => p.ControlType);
			HasMany(p => p.PropertyValues).Cascade.AllDeleteOrphan();
			Map(p => p.DisplayOrder);
			Map(p => p.CreateTime);
			Map(p => p.LastUpdated);
			Map(p => p.Deleted);
			Map(p => p.Remark).Length(0xffff);
		}
	}
}
