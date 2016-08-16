using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Database {
	/// <summary>
	/// 通用配置
	/// </summary>
	[ExportMany]
	public class GenericConfig : IEntity<long>, IEntityMappingProvider<GenericConfig> {
		/// <summary>
		/// 主键，没有意义
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 所属插件 + 配置键名
		/// </summary>
		public virtual string Key { get; set; }
		/// <summary>
		/// 配置值（json）
		/// </summary>
		public virtual string Value { get; set; }
		/// <summary>
		/// 最后更新时间
		/// </summary>
		public virtual DateTime LastUpdated { get; set; }

		/// <summary>
		/// 通用配置的数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<GenericConfig> builder) {
			builder.Id(c => c.Id);
			builder.Map(c => c.Key, new EntityMappingOptions() {
				Column = "key_", Unique = true, Length = 255
			});
			builder.Map(c => c.Value);
			builder.Map(c => c.LastUpdated);
		}
	}
}
