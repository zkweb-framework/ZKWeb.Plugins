using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Shopping.Order.src.ScheduledTasks {
	/// <summary>
	/// 自动确认订单
	/// </summary>
	[ExportMany, SingletonReuse]
	public class OrderAutoConfirmer : IScheduledTaskExecutor {
		/// <summary>
		/// 任务键名
		/// </summary>
		public string Key { get { return "Shopping.Order.OrderAutoConfirmer"; } }

		/// <summary>
		/// 每小时执行一次
		/// </summary>
		public bool ShouldExecuteNow(DateTime lastExecuted) {
			return ((DateTime.UtcNow - lastExecuted).TotalHours > 1.0);
		}

		/// <summary>
		/// 自动确认订单
		/// </summary>
		public void Execute() {
			// TODO: 完成这里的功能
		}
	}
}
