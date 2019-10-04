using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Base.src.Components.ScheduledTasks.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Services {
	/// <summary>
	/// 定时任务管理器
	/// 流程
	/// 收集所有IScheduledTaskExecutor
	/// 每分钟
	/// - 获取数据库中的最后执行时间
	/// - 调用ShouldExecuteNow函数
	/// - 以事务更新数据库中的最后执行时间
	/// - 更新成功时调用Execute
	/// - Execute抛出例外时记录到日志
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ScheduledTaskManager :
		DomainServiceBase<ScheduledTask, string>, IDisposable {
		/// <summary>
		/// 定时任务执行器的列表
		/// </summary>
		private Lazy<IEnumerable<IScheduledTaskExecutor>> Executors =
			new Lazy<IEnumerable<IScheduledTaskExecutor>>(() =>
			Application.Ioc.ResolveMany<IScheduledTaskExecutor>().ToList());
		private volatile bool _keepalive = true;
		private ManualResetEvent _exited = new ManualResetEvent(false);

		/// <summary>
		/// 创建执行定时任务的线程
		/// </summary>
		public ScheduledTaskManager() {
			var logManager = Application.Ioc.Resolve<LogManager>();
			var thread = new Thread(() => {
				// 每分钟调查一次是否有需要执行的任务
				try {
					while (_keepalive) {
						for (int x = 0; x < 60; ++x) {
							Thread.Sleep(TimeSpan.FromSeconds(1));
							if (!_keepalive) {
								return;
							}
						}
						// 枚举并处理定时任务
						foreach (var executor in Executors.Value) {
							if (!_keepalive) {
								return;
							}
							try {
								HandleTask(executor);
							} catch (Exception e) {
								logManager.LogError(e.ToString());
							}
						}
					}
				} finally {
					_exited.Set();
				}
			});
			thread.IsBackground = true;
			thread.Start();
			logManager.LogInfo("Scheduled task manager thread started");
		}

		/// <summary>
		/// 处理单个定时任务
		/// </summary>
		/// <param name="executor">定时任务执行器</param>
		protected virtual void HandleTask(IScheduledTaskExecutor executor) {
			var uow = UnitOfWork;
			using (uow.Scope()) {
				// 从数据库获取任务的最后执行时间，判断是否需要立刻执行
				uow.Context.BeginTransaction();
				var task = Get(executor.Key);
				var lastExecuted = task != null ? task.UpdateTime : DateTime.MinValue;
				if (!executor.ShouldExecuteNow(lastExecuted)) {
					return;
				}
				// 执行前使用事务更新数据库中的执行时间，防止多个进程托管同一个网站时重复执行
				task = task ?? new ScheduledTask() { Id = executor.Key };
				Save(ref task, t => t.UpdateTime = DateTime.UtcNow);
				uow.Context.FinishTransaction();
			}
			// 执行定时任务
			executor.Execute();
		}

		/// <summary>
		/// 停止执行定时任务的线程
		/// </summary>
		public virtual void Dispose() {
			_keepalive = false;
			if (_exited.WaitOne(TimeSpan.FromMinutes(5))) {
				var logManager = Application.Ioc.Resolve<LogManager>();
				logManager.LogInfo("Scheduled task manager thread stopped");
			} else {
				throw new TimeoutException("Wait for scheduled task manager thread stop failed");
			}
		}
	}
}
