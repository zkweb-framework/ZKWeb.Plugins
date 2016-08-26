using System.Linq;
using ZKWeb.Database;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Filters.Interfaces {
	/// <summary>
	/// 查询过滤器
	/// </summary>
	public interface IEntityQueryFilter {
		/// <summary>
		/// 过滤查询
		/// </summary>
		/// <typeparam name="TEntity">实体类型</typeparam>
		/// <typeparam name="TPrimaryKey">主键类型</typeparam>
		/// <param name="query">查询对象</param>
		/// <returns></returns>
		IQueryable<TEntity> Filter<TEntity, TPrimaryKey>(IQueryable<TEntity> query)
			where TEntity : class, IEntity<TPrimaryKey>;
	}
}
