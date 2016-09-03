using DotLiquid;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Domain.Entities {
	/// <summary>
	/// 商品类目
	/// </summary>
	[ExportMany]
	public class ProductCategory :
		IHaveCreateTime, IHaveUpdateTime, IHaveDeleted,
		ILiquidizable, IEntity<Guid>, IEntityMappingProvider<ProductCategory> {
		/// <summary>
		/// 类目Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 类目名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 类目下的属性，多对多
		/// </summary>
		public virtual ISet<ProductProperty> Properties { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime UpdateTime { get; set; }
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
		public ProductCategory() {
			Properties = new HashSet<ProductProperty>();
		}

		/// <summary>
		/// 支持描画到模板
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return new { Id, Name, Properties };
		}

		/// <summary>
		/// 显示名称
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Name;
		}

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<ProductCategory> builder) {
			builder.Id(c => c.Id);
			builder.Map(c => c.Name);
			builder.HasManyToMany(c => c.Properties);
			builder.Map(c => c.CreateTime);
			builder.Map(c => c.UpdateTime);
			builder.Map(c => c.Deleted);
			builder.Map(c => c.Remark);
		}
	}
}
