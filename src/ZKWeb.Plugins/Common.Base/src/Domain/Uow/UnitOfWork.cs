using System;
using System.Threading;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Interfaces;
using ZKWebStandard.Collections;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Uow {
	/// <summary>
	/// 工作单元
	/// </summary>
	[ExportMany, SingletonReuse]
	internal class UnitOfWork : IUnitOfWork {
		/// <summary>
		/// 当前的数据库上下文
		/// </summary>
		public virtual IDatabaseContext Context {
			get {
				var context = ThreadContext.Value;
				if (context == null)
					throw new InvalidOperationException("Context is not available, please call Scope() first");
				return context;
			}
		}
		private ThreadLocal<IDatabaseContext> ThreadContext { get; set; }

		/// <summary>
		/// 创建数据库上下文
		/// </summary>
		/// <returns></returns>
		protected virtual IDatabaseContext CreateContext() {
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			return databaseManager.CreateContext();
		}

		/// <summary>
		/// 在指定的范围内使用工作单元
		/// 最外层的工作单元负责创建和销毁数据库上下文
		/// </summary>
		/// <returns></returns>
		public virtual IDisposable Scope() {
			var isRootUow = ThreadContext.Value == null;
			if (isRootUow) {
				var context = CreateContext();
				ThreadContext.Value = context;
				return new SimpleDisposable(() => {
					context.Dispose();
					ThreadContext.Value = null;
				});
			}
			return new SimpleDisposable(() => { });
		}
	}
}
