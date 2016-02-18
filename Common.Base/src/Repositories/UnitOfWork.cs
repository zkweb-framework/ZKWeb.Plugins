using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Database;

namespace ZKWeb.Plugins.Common.Base.src.Repositories {
	/// <summary>
	/// 开启事务处理多个数据仓储的查询或改动
	/// </summary>
	public static class UnitOfWork {
		/// <summary>
		/// 执行读取数据使用的工作
		/// </summary>
		/// <param name="action">工作内容</param>
		public static void Read(Action<DatabaseContext> action) {
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.GetContext()) {
				action(context);
			}
		}

		/// <summary>
		/// 执行修改数据使用的工作
		/// </summary>
		/// <param name="action">工作内容</param>
		public static void Write(Action<DatabaseContext> action) {
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.GetContext()) {
				action(context);
				context.SaveChanges();
			}
		}

		/// <summary>
		/// 执行读取数据使用的工作
		/// 同时创建指定数据类型的仓储
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="action">工作内容</param>
		public static void ReadData<TData>(Action<GenericRepository<TData>> action)
			where TData : class {
			Read(context => {
				var repository = RepositoryResolver.Resolve<TData>(context);
				action(repository);
			});
		}

		/// <summary>
		/// 执行读取数据使用的工作
		/// 同时创建指定类型的仓储
		/// </summary>
		/// <typeparam name="TRepository">数据仓储类型</typeparam>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="action">工作内容</param>
		public static void ReadData<TRepository, TData>(Action<TRepository> action)
			where TRepository : GenericRepository<TData>
			where TData : class {
			Read(context => {
				var repository = RepositoryResolver.Resolve<TRepository, TData>(context);
				action(repository);
			});
		}

		/// <summary>
		/// 执行修改数据使用的工作
		/// 同时创建指定数据类型的仓储
		/// </summary>
		/// <typeparam name="TData">数据仓储类型</typeparam>
		/// <param name="action">工作内容</param>
		public static void WriteData<TData>(Action<GenericRepository<TData>> action)
			where TData : class {
			Write(context => {
				var repository = RepositoryResolver.Resolve<TData>(context);
				action(repository);
			});
		}

		/// <summary>
		/// 执行修改数据使用的工作
		/// 同时创建指定数据类型的仓储
		/// </summary>
		/// <typeparam name="TRepository">数据仓储类型</typeparam>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="action">工作内容</param>
		public static void WriteData<TRepository, TData>(Action<TRepository> action)
			where TRepository : GenericRepository<TData>
			where TData : class {
			Write(context => {
				var repository = RepositoryResolver.Resolve<TRepository, TData>(context);
				action(repository);
			});
		}
	}
}
