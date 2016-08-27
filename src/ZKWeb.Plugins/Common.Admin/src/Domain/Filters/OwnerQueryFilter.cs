using System;
using System.Linq;
using System.Reflection;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Filters.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;

namespace ZKWeb.Plugins.Common.Admin.src.Domain.Filters {
	/// <summary>
	/// 根据数据所属用户过滤查询
	/// 这个过滤器默认不启用，有需要请手动启用
	/// </summary>
	public class OwnerQueryFilter : IEntityQueryFilter {
		/// <summary>
		/// 数据应当属于的用户Id
		/// </summary>
		public Guid ExceptedUserId { get; set; }

		/// <summary>
		/// 初始化
		/// 数据应当属于的用户Id默认等于当前登录用户Id
		/// </summary>
		public OwnerQueryFilter() {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			ExceptedUserId = sessionManager.GetSession().ReleatedId;
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="exceptedUserId">数据应当属于的用户Id</param>
		public OwnerQueryFilter(Guid exceptedUserId) {
			ExceptedUserId = exceptedUserId;
		}

		/// <summary>
		/// 类型特征
		/// </summary>
		protected static class TypeTrait<TEntity> {
			/// <summary>
			/// 类型包含所属用户
			/// </summary>
			public readonly static bool HaveOwner =
				typeof(IHaveOwner).GetTypeInfo().IsAssignableFrom(typeof(TEntity));
		}

		/// <summary>
		/// 过滤所属用户
		/// </summary>
		public virtual IQueryable<TEntity> Filter<TEntity, TPrimaryKey>(IQueryable<TEntity> query)
			where TEntity : class, IEntity<TPrimaryKey> {
			if (TypeTrait<TEntity>.HaveOwner) {
				query = query.Where(e => ((IHaveOwner)e).Owner.Id == ExceptedUserId);
			}
			return query;
		}
	}
}
