using DotLiquid;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Shopping.Logistics.src.Domain.Structs;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Logistics.src.Domain.Entities {
	/// <summary>
	/// 物流
	/// </summary>
	[ExportMany]
	public class Logistics :
		IHaveCreateTime, IHaveUpdateTime, IHaveDeleted,
		ILiquidizable, IEntity<Guid>, IEntityMappingProvider<Logistics> {
		/// <summary>
		/// 物流Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 物流名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 物流类型
		/// </summary>
		public virtual string Type { get; set; }
		/// <summary>
		/// 物流费用计算规则
		/// </summary>
		public virtual List<PriceRule> PriceRules { get; set; }
		/// <summary>
		/// 所有人，没有时等于null
		/// </summary>
		public virtual User Owner { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 最后更新时间
		/// </summary>
		public virtual DateTime UpdateTime { get; set; }
		/// <summary>
		/// 是否已删除
		/// </summary>
		public virtual bool Deleted { get; set; }
		/// <summary>
		/// 显示顺序，从小到大
		/// </summary>
		public virtual long DisplayOrder { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public virtual string Remark { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public Logistics() {
			PriceRules = new List<PriceRule>();
			DisplayOrder = 10000;
		}

		/// <summary>
		/// 允许描画到模板
		/// </summary>
		/// <returns></returns>
		public virtual object ToLiquid() {
			return new { Id, Name, Type };
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
		public virtual void Configure(IEntityMappingBuilder<Logistics> builder) {
			builder.Id(l => l.Id);
			builder.Map(l => l.Name);
			builder.Map(l => l.Type, new EntityMappingOptions() {
				Index = "Idx_Type"
			});
			builder.Map(l => l.PriceRules, new EntityMappingOptions() {
				WithSerialization = true
			});
			builder.References(l => l.Owner);
			builder.Map(l => l.CreateTime);
			builder.Map(l => l.UpdateTime);
			builder.Map(l => l.Deleted);
			builder.Map(l => l.DisplayOrder);
			builder.Map(l => l.Remark);
		}
	}
}
