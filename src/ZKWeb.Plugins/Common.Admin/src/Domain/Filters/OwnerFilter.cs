using System;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.TypeTraits;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Filters.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;

namespace ZKWeb.Plugins.Common.Admin.src.Domain.Filters {
	/// <summary>
	/// 根据数据所属用户过滤查询和操作
	/// 这个过滤器默认不启用，有需要请手动启用
	/// </summary>
	public class OwnerFilter : IEntityQueryFilter, IEntityOperationFilter {
		/// <summary>
		/// 数据应当属于的用户Id
		/// </summary>
		public Guid ExceptedOwnerId { get; set; }

		/// <summary>
		/// 初始化
		/// 数据应当属于的用户Id默认等于当前登录用户Id
		/// </summary>
		public OwnerFilter() {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			ExceptedOwnerId = sessionManager.GetSession().ReleatedId ?? Guid.Empty;
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="exceptedOwnerId">数据应当属于的用户Id</param>
		public OwnerFilter(Guid exceptedOwnerId) {
			ExceptedOwnerId = exceptedOwnerId;
		}

		/// <summary>
		/// 过滤查询
		/// </summary>
		IQueryable<TEntity> IEntityQueryFilter.FilterQuery<TEntity, TPrimaryKey>(
			IQueryable<TEntity> query) {
			if (OwnerTypeTrait<TEntity>.HaveOwner) {
				query = query.Where(e => ((IHaveOwner)e).Owner.Id == ExceptedOwnerId);
			}
			return query;
		}

		/// <summary>
		/// 过滤查询条件
		/// </summary>
		Expression<Func<TEntity, bool>> IEntityQueryFilter.FilterPredicate<TEntity, TPrimaryKey>(
			Expression<Func<TEntity, bool>> predicate) {
			if (OwnerTypeTrait<TEntity>.HaveOwner) {
				var paramExpr = predicate.Parameters[0];
				var memberExpr = Expression.Property(
					Expression.Property(paramExpr, nameof(IHaveOwner.Owner)),
					nameof(IEntity<Guid>.Id));
				var body = Expression.AndAlso(
					predicate.Body,
					Expression.Equal(memberExpr, Expression.Constant(ExceptedOwnerId)));
				predicate = Expression.Lambda<Func<TEntity, bool>>(body, paramExpr);
			}
			return predicate;
		}

		/// <summary>
		/// 保存时检查所属用户
		/// </summary>
		void IEntityOperationFilter.FilterSave<TEntity, TPrimaryKey>(TEntity entity) {
			if (!OwnerTypeTrait<TEntity>.HaveOwner) {
				return;
			}
			var e = ((IHaveOwner)entity);
			if (e.Owner == null && ExceptedOwnerId == Guid.Empty) {
				// 未登陆用户保存数据，不需要设置
			} else if (e.Owner == null && ExceptedOwnerId != Guid.Empty) {
				// 已登陆用户保存数据，设置所属用户，注意这里会受查询过滤器的影响
				var repository = Application.Ioc.Resolve<IRepository<User, Guid>>();
				var user = repository.Get(u => u.Id == ExceptedOwnerId);
				if (user == null) {
					throw new BadRequestException(new T("Set entity owner failed, user not found"));
				}
				e.Owner = user;
			} else if (e.Owner != null && e.Owner.Id != ExceptedOwnerId) {
				// 已登陆用户保存数据，但数据不属于这个用户
				throw new ForbiddenException(string.Format(
					new T("Action require the ownership of {0}: {1}"),
					new T(typeof(TEntity).Name), entity.Id));
			}
		}

		/// <summary>
		/// 删除时检查所属用户
		/// </summary>
		void IEntityOperationFilter.FilterDelete<TEntity, TPrimaryKey>(TEntity entity) {
			if (!OwnerTypeTrait<TEntity>.HaveOwner) {
				return;
			}
			var e = ((IHaveOwner)entity);
			if (e.Owner == null) {
				// 删除没有所属用户的数据，不需要拦截
			} else if (e.Owner != null && e.Owner.Id != ExceptedOwnerId) {
				// 已登陆用户删除数据，但数据不属于这个用户
				throw new ForbiddenException(string.Format(
					new T("Action require the ownership of {0}: {1}"),
					new T(typeof(TEntity).Name), entity.Id));
			}
		}
	}
}
