using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 定时任务执行器
	/// </summary>
	public interface IScheduledTaskExecutor {
		/// <summary>
		/// 任务键名
		/// </summary>
		string Key { get; }
		/// <summary>
		/// 判断是否应该立刻执行任务
		/// </summary>
		/// <param name="lastExecuted">最后一次执行任务的时间，首次调用是DateTime.MinValue</param>
		/// <returns></returns>
		bool ShouldExecuteNow(DateTime lastExecuted);
		/// <summary>
		/// 执行任务
		/// </summary>
		void Execute();
	}
}
