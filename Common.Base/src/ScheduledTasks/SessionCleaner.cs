using System;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.ScheduledTasks {
	/// <summary>
	/// 会话清理器
	/// 每小时删除一次过期的会话
	/// </summary>
	[ExportMany, SingletonReuse]
	public class SessionCleaner : IScheduledTaskExecutor {
		/// <summary>
		/// 任务键名
		/// </summary>
		public string Key { get { return "Common.Base.SessionCleaner"; } }

		/// <summary>
		/// 每小时执行一次
		/// </summary>
		public bool ShouldExecuteNow(DateTime lastExecuted) {
			return ((DateTime.UtcNow - lastExecuted).TotalHours > 1.0);
		}

		/// <summary>
		/// 删除过期的会话
		/// </summary>
		public void Execute() {
			var count = UnitOfWork.WriteData<Session, long>(r => {
				var now = DateTime.UtcNow;
				return r.BatchDelete(s => s.Expires < now);
			});
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogInfo(string.Format(
				"SessionCleaner executed, {0} sessions removed", count));
		}
	}
}
