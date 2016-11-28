using System;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Base.src.Components.ScheduledTasks.Interfaces;
using ZKWeb.Plugins.Common.GenericRecord.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericRecord.src.Components.ScheduledTasks {
	/// <summary>
	/// 清理过期的通用记录
	/// </summary>
	[ExportMany]
	public class GenericRecordCleaner : IScheduledTaskExecutor {
		/// <summary>
		/// 任务键名
		/// </summary>
		public string Key { get { return "Common.GenericRecord.GenericRecordCleaner"; } }

		/// <summary>
		/// 每小时执行一次
		/// </summary>
		public bool ShouldExecuteNow(DateTime lastExecuted) {
			return ((DateTime.UtcNow - lastExecuted).TotalHours > 1.0);
		}

		/// <summary>
		/// 清理过期的通用记录
		/// </summary>
		public void Execute() {
			var recordManager = Application.Ioc.Resolve<GenericRecordManager>();
			var count = recordManager.ClearExpiredRecords();
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogInfo(string.Format(
				"GenericRecordCleaner executed, {0} records removed", count));
		}
	}
}
