using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Database;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Plugins.Common.Base.src.Model;

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
		/// <param name="lastExecuted"></param>
		/// <returns></returns>
		public bool ShouldExecuteNow(DateTime lastExecuted) {
			return ((DateTime.UtcNow - lastExecuted).TotalHours > 1.0);
		}

		/// <summary>
		/// 删除过期的会话
		/// </summary>
		public void Execute() {
			long count;
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.GetContext()) {
				var now = DateTime.UtcNow;
				count = context.DeleteWhere<Session>(s => s.Expires < now);
				context.SaveChanges();
			}
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogInfo(string.Format(
				"SessionCleaner executed, {0} sessions removed", count));
		}
	}
}
