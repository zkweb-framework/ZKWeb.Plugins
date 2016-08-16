using System.Linq;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Base.src.Repositories {
	/// <summary>
	/// 用于获取数据仓储实例的静态类
	/// </summary>
	public static class RepositoryResolver {
		/// <summary>
		/// 获取指定数据的数据仓储
		/// 如果有在Ioc中注册的继承类，例如
		/// [ExportMany]
		/// class UserRepository : GenericRepository[TData] { }
		/// 则返回这个类的实例，否则使用默认的仓储对象
		/// 如果注册了多个，返回最后一个注册的类型
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="context">数据库上下文</param>
		/// <returns></returns>
		public static GenericRepository<TData> Resolve<TData>(IDatabaseContext context)
			where TData : class, IEntity {
			var repository = Application.Ioc.ResolveMany<GenericRepository<TData>>().LastOrDefault();
			repository = repository ?? new GenericRepository<TData>();
			repository.Context = context;
			return repository;
		}

		/// <summary>
		/// 获取指定类型的数据仓储
		/// 如果注册了多个，返回最后一个注册的实例
		/// </summary>
		/// <typeparam name="TRepository">数据仓储类型</typeparam>
		/// <param name="context">数据库上下文</param>
		/// <returns></returns>
		public static TRepository ResolveRepository<TRepository>(IDatabaseContext context)
			where TRepository : IRepository {
			var repository = Application.Ioc.ResolveMany<TRepository>().Last();
			repository.Context = context;
			return repository;
		}
	}
}
