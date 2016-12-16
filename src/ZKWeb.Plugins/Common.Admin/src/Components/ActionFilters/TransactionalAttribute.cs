using System;
using System.Data;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Interfaces;
using ZKWeb.Web;

namespace ZKWeb.Plugins.Common.Admin.src.Components.ActionFilters {
	/// <summary>
	/// 启用事务的属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class TransactionalAttribute : ActionFilterAttribute {
		/// <summary>
		/// 隔离等级
		/// </summary>
		public IsolationLevel? IsolationLevel { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public TransactionalAttribute(IsolationLevel? isolationLevel = null) {
			IsolationLevel = IsolationLevel;
		}

		/// <summary>
		/// 运行时包装在事务内
		/// </summary>
		public override Func<IActionResult> Filter(Func<IActionResult> action) {
			return () => {
				var uow = Application.Ioc.Resolve<IUnitOfWork>();
				using (uow.Scope()) {
					uow.Context.BeginTransaction(IsolationLevel);
					var result = action();
					uow.Context.FinishTransaction();
					return result;
				}
			};
		}
	}
}
