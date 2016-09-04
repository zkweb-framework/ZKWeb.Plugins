using System;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.TypeTraits;
using ZKWeb.Plugins.Common.Base.src.Domain.Filters.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Filters {
	/// <summary>
	/// 根据删除状态过滤查询
	/// 字段没有删除状态时返回原查询
	/// </summary>
	[ExportMany]
	public class DeletedFilter : IEntityQueryFilter {
		/// <summary>
		/// true: 查询已删除的对象
		/// false: 查询未删除的对象
		/// </summary>
		public bool Deleted { get; protected set; }

		/// <summary>
		/// 初始化
		/// 默认查询未删除的对象
		/// </summary>
		public DeletedFilter() {
			Deleted = false;
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="deleted">查询已删除或未删除对象</param>
		public DeletedFilter(bool deleted) {
			Deleted = deleted;
		}

		/// <summary>
		/// 过滤查询
		/// </summary>
		IQueryable<TEntity> IEntityQueryFilter.FilterQuery<TEntity, TPrimaryKey>(
			IQueryable<TEntity> query) {
			if (DeletedTypeTrait<TEntity>.HaveDeleted) {
				query = query.Where(e => ((IHaveDeleted)e).Deleted == Deleted);
			}
			return query;
		}

		/// <summary>
		/// 过滤查询条件
		/// </summary>
		Expression<Func<TEntity, bool>> IEntityQueryFilter.FilterPredicate<TEntity, TPrimaryKey>(
			Expression<Func<TEntity, bool>> predicate) {
			if (DeletedTypeTrait<TEntity>.HaveDeleted) {
				var paramExpr = predicate.Parameters[0];
				var memberExpr = Expression.Property(paramExpr, nameof(IHaveDeleted.Deleted));
				var body = Expression.AndAlso(
					predicate.Body,
					Expression.Equal(memberExpr, Expression.Constant(Deleted)));
				predicate = Expression.Lambda<Func<TEntity, bool>>(body, paramExpr);
			}
			return predicate;
		}
	}
}
