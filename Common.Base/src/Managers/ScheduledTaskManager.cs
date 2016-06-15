using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using ZKWeb;
using ZKWeb.Database;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Managers {
	/// <summary>
	/// 定时任务管理器
	/// 流程
	/// 收集所有IScheduledTaskExecutor
	/// 每分钟
	///		获取数据库中的lastExecuted
	///		调用ShouldExecuteNow函数
	///		以事务更新数据库中的LastExecuted
	///		更新成功时调用Execute
	///		Execute抛出例外时记录到日志
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ScheduledTaskManager {
		/// <summary>
		/// 定时任务执行器的列表
		/// </summary>
		private Lazy<IEnumerable<IScheduledTaskExecutor>> Executors =
			new Lazy<IEnumerable<IScheduledTaskExecutor>>(() =>
			Application.Ioc.ResolveMany<IScheduledTaskExecutor>());

		/// <summary>
		/// 创建执行定时任务的线程
		/// 线程会在AppDomain.Unload的时候收到AbortException
		/// </summary>
		public ScheduledTaskManager() {
			var logManager = Application.Ioc.Resolve<LogManager>();
			var thread = new Thread(() => {
				// 每分钟调查一次是否有需要执行的任务
				while (true) {
					Thread.Sleep(TimeSpan.FromMinutes(1));
					// 枚举并处理定时任务
					foreach (var executor in Executors.Value) {
						try {
							HandleTask(executor);
						} catch (Exception e) {
							logManager.LogError(e.ToString());
						}
					}
				}
			});
			thread.IsBackground = true;
			thread.Start();
		}

		/// <summary>
		/// 处理单个定时任务
		/// </summary>
		/// <param name="executor">定时任务执行器</param>
		public void HandleTask(IScheduledTaskExecutor executor) {
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.GetContext()) {
				// 从数据库获取任务的最后执行时间，判断是否需要立刻执行
				var task = context.Get<ScheduledTask>(t => t.Key == executor.Key);
				if (!executor.ShouldExecuteNow(task != null ? task.LastExecuted : DateTime.MinValue)) {
					return;
				}
				// 执行前使用事务更新数据库中的LastExecuted，防止多个进程托管同一个网站时重复执行
				// 如果重复执行Save应该会抛出例外（实际捕捉到例外后再添加catch处理）
				if (task == null) {
					task = new ScheduledTask() { Key = executor.Key, CreateTime = DateTime.UtcNow };
				}
				context.Save(ref task, t => t.LastExecuted = DateTime.UtcNow);
				context.SaveChanges();
			}
			// 执行定时任务
			executor.Execute();
		}
	}
}
