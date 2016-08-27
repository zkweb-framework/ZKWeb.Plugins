using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Filters.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Interfaces;
using ZKWebStandard.Collections;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Uow {
	/// <summary>
	/// 工作单元
	/// </summary>
	[ExportMany, SingletonReuse]
	public class UnitOfWork : IUnitOfWork {
		/// <summary>
		/// 同一个工作单元区域使用的数据
		/// </summary>
		private class ScopeData : IDisposable {
			/// <summary>
			/// 数据库上下文
			/// </summary>
			public IDatabaseContext Context { get; set; }
			/// <summary>
			/// 默认的查询过滤器
			/// </summary>
			public IList<IEntityQueryFilter> QueryFilters { get; set; }
			/// <summary>
			/// 默认的保存过滤器
			/// </summary>
			public IList<IEntitySaveFilter> SaveFilters { get; set; }

			/// <summary>
			/// 初始化
			/// </summary>
			public ScopeData() {
				var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
				Context = databaseManager.CreateContext();
				QueryFilters = Application.Ioc.ResolveMany<IEntityQueryFilter>().ToList();
				SaveFilters = Application.Ioc.ResolveMany<IEntitySaveFilter>().ToList();
			}

			/// <summary>
			/// 释放数据
			/// </summary>
			~ScopeData() {
				Dispose();
			}

			/// <summary>
			/// 释放数据
			/// </summary>
			public void Dispose() {
				Context?.Dispose();
				Context = null;
			}
		}

		/// <summary>
		/// 同一个工作单元区域使用的数据
		/// </summary>
		private ThreadLocal<ScopeData> Data { get; set; }

		/// <summary>
		/// 当前的数据库上下文
		/// </summary>
		public IDatabaseContext Context {
			get {
				var context = Data.Value?.Context;
				if (context == null) {
					throw new InvalidOperationException("Please call Scope() first");
				}
				return context;
			}
		}

		/// <summary>
		/// 当前的查询过滤器列表
		/// </summary>
		public IList<IEntityQueryFilter> QueryFilters {
			get {
				var filters = Data.Value?.QueryFilters;
				if (filters == null) {
					throw new InvalidOperationException("Please call Scope() first");
				}
				return filters;
			}
			set {
				if (value == null) {
					throw new ArgumentNullException("value");
				} else if (Data.Value == null) {
					throw new InvalidOperationException("Please call Scope() first");
				}
				Data.Value.QueryFilters = value;
			}
		}

		/// <summary>
		/// 当前的保存过滤器列表
		/// </summary>
		public IList<IEntitySaveFilter> SaveFilters {
			get {
				var filters = Data.Value?.SaveFilters;
				if (filters == null) {
					throw new InvalidOperationException("Please call Scope() first");
				}
				return filters;
			}
			set {
				if (value == null) {
					throw new ArgumentNullException("value");
				} else if (Data.Value == null) {
					throw new InvalidOperationException("Please call Scope() first");
				}
				Data.Value.SaveFilters = value;
			}
		}

		/// <summary>
		/// 初始化
		/// </summary>
		public UnitOfWork() {
			Data = new ThreadLocal<ScopeData>();
		}

		/// <summary>
		/// 在指定的范围内使用工作单元
		/// 最外层的工作单元负责创建和销毁数据
		/// </summary>
		/// <returns></returns>
		public IDisposable Scope() {
			var isRootUow = Data.Value == null;
			if (isRootUow) {
				var data = new ScopeData();
				Data.Value = data;
				return new SimpleDisposable(() => {
					data.Dispose();
					Data.Value = null;
				});
			}
			return new SimpleDisposable(() => { });
		}
	}
}
