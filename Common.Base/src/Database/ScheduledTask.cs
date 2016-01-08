using DryIocAttributes;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Model;

namespace ZKWeb.Plugins.Common.Base.src.Database {
	/// <summary>
	/// 定时任务
	/// </summary>
	public class ScheduledTask {
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
	}

	/// <summary>
	/// 定时任务的数据库结构
	/// </summary>
	[ExportMany]
	public class ScheduledTaskMap : ClassMap<ScheduledTask> {
		/// <summary>
		/// 初始化
		/// key在sqlserver中是关键字，需要改名
		/// </summary>
		public ScheduledTaskMap() {
			Id(t => t.Id);
			Map(t => t.Key).Column("key_").Length(255).Unique();
			Map(t => t.CreateTime);
			Map(t => t.LastExecuted);
		}
	}
}
