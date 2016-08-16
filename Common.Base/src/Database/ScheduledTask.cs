using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Database {
	/// <summary>
	/// 定时任务
	/// </summary>
	[ExportMany]
	public class ScheduledTask : IEntity<long>, IEntityMappingProvider<ScheduledTask> {
		/// <summary>
		/// 主键，没有意义
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 所属插件 + 任务键名
		/// </summary>
		public virtual string Key { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 最后一次执行的时间
		/// </summary>
		public virtual DateTime LastExecuted { get; set; }

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<ScheduledTask> builder) {
			builder.Id(t => t.Id);
			builder.Map(t => t.Key, new EntityMappingOptions() {
				Column = "key_", Unique = true, Length = 255
			});
			builder.Map(t => t.CreateTime);
			builder.Map(t => t.LastExecuted);
		}
	}
}
