using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Entities {
	/// <summary>
	/// 通用配置
	/// </summary>
	[ExportMany]
	public class GenericConfig :
		IEntity<string>, IHaveCreateTime, IHaveUpdateTime,
		IEntityMappingProvider<GenericConfig> {
		/// <summary>
		/// 主键，配置名称
		/// </summary>
		public virtual string Id { get; set; }
		/// <summary>
		/// 配置值（json）
		/// </summary>
		public virtual string Value { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime UpdateTime { get; set; }

		/// <summary>
		/// 配置的数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<GenericConfig> builder) {
			builder.Id(c => c.Id);
			builder.Map(c => c.Value);
			builder.Map(c => c.UpdateTime);
		}
	}
}
