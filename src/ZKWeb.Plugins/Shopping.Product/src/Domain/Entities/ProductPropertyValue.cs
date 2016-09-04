using DotLiquid;
using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Domain.Entities {
	/// <summary>
	/// 商品属性值
	/// </summary>
	[ExportMany]
	public class ProductPropertyValue :
		IHaveCreateTime, IHaveUpdateTime,
		ILiquidizable, IEntity<Guid>, IEntityMappingProvider<ProductPropertyValue> {
		/// <summary>
		/// 属性值Id
		/// </summary>
		public virtual Guid Id { get; set; }
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
		public virtual DateTime UpdateTime { get; set; }
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

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<ProductPropertyValue> builder) {
			builder.Id(p => p.Id);
			builder.Map(p => p.Name);
			builder.References(p => p.Property);
			builder.Map(p => p.DisplayOrder);
			builder.Map(p => p.CreateTime);
			builder.Map(p => p.UpdateTime);
			builder.Map(p => p.Remark);
		}
	}
}
