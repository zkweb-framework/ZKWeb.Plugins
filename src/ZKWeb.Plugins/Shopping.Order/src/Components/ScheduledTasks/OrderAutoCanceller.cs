using System;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Base.src.Components.ScheduledTasks.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.ScheduledTasks {
	/// <summary>
	/// 自动取消未付款的订单
	/// </summary>
	[ExportMany]
	public class OrderAutoCanceller : IScheduledTaskExecutor {
		/// <summary>
		/// 任务键名
		/// </summary>
		public string Key { get { return "Shopping.Order.OrderAutoCanceller"; } }

		/// <summary>
		/// 每小时执行一次
		/// </summary>
		public bool ShouldExecuteNow(DateTime lastExecuted) {
			return ((DateTime.UtcNow - lastExecuted).TotalHours > 1.0);
		}

		/// <summary>
		/// 自动取消未付款的订单
		/// </summary>
		public void Execute() {
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var count = orderManager.AutoCancelOrder();
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogInfo(string.Format(
				"OrderAutoCanceller executed, {0} order cancelled", count));
		}
	}
}
