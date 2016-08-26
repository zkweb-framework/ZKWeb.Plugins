using System.Linq;
using System.Reflection;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Filters.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Filters {
	/// <summary>
	/// 根据删除状态过滤查询
	/// 字段没有删除状态时返回原查询
	/// </summary>
	[ExportMany]
	public class QueryFilterForDeleted : IEntityQueryFilter {
		/// <summary>
		/// true: 查询已删除的对象
		/// false: 查询未删除的对象
		/// </summary>
		public bool Deleted { get; protected set; }

		/// <summary>
		/// 初始化
		/// 默认查询未删除的对象
		/// </summary>s
		public QueryFilterForDeleted() {
			Deleted = false;
		}

		/// <summary>
		/// 类型特征
		/// </summary>
		protected static class TypeTrait<TEntity> {
			/// <summary>
			/// 类型包含已删除标记
			/// </summary>
			public readonly static bool HaveDeleted =
				typeof(IHaveDeleted).GetTypeInfo().IsAssignableFrom(typeof(TEntity));
		}

		/// <summary>
		/// 过滤查询
		/// </summary>
		public IQueryable<TEntity> Filter<TEntity, TPrimaryKey>(IQueryable<TEntity> query)
			where TEntity : class, IEntity<TPrimaryKey> {
			if (TypeTrait<TEntity>.HaveDeleted) {
				query = query.Where(e => ((IHaveDeleted)e).Deleted == Deleted);
			}
			return query;
		}
	}
}
