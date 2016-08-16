using DotLiquid;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Database {
	/// <summary>
	/// 商品属性
	/// </summary>
	[ExportMany]
	public class ProductProperty :
		ILiquidizable, IEntity<long>, IEntityMappingProvider<ProductProperty> {
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

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<ProductProperty> builder) {
			builder.Id(p => p.Id);
			builder.Map(p => p.Name);
			builder.Map(p => p.IsSalesProperty);
			builder.Map(p => p.ControlType);
			builder.HasMany(p => p.PropertyValues);
			builder.Map(p => p.DisplayOrder);
			builder.Map(p => p.CreateTime);
			builder.Map(p => p.LastUpdated);
			builder.Map(p => p.Deleted);
			builder.Map(p => p.Remark);
		}
	}
}
