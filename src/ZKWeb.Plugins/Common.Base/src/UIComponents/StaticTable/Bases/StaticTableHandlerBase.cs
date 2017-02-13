using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.TypeTraits;
using ZKWeb.Plugins.Common.Base.src.Domain.Filters;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Extensions;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Interfaces;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Bases {
	/// <summary>
	/// 静态表格处理器的基础类
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	public abstract class StaticTableHandlerBase<TEntity, TPrimaryKey> :
		IStaticTableHandler<TEntity, TPrimaryKey>
		where TEntity : class, IEntity<TPrimaryKey> {
		/// <summary>
		/// 包装查询函数
		/// 默认根据搜索参数中的Deleted过滤已删除或未删除的对象
		/// </summary>
		public virtual Func<TResult> WrapQueryMethod<TResult>(
			StaticTableSearchRequest request, Func<TResult> queryMethod) {
			var uow = Application.Ioc.Resolve<IUnitOfWork>();
			var deleted = request.Conditions.GetOrDefault<bool>("Deleted");
			return () => {
				using (uow.DisableQueryFilter(typeof(DeletedFilter)))
				using (uow.EnableQueryFilter(new DeletedFilter() { Deleted = deleted })) {
					return queryMethod();
				}
			};
		}

		/// <summary>
		/// 过滤数据
		/// 默认不过滤数据
		/// </summary>
		public virtual void OnQuery(
			StaticTableSearchRequest request, ref IQueryable<TEntity> query) { }

		/// <summary>
		/// 排序数据
		/// 默认更新时间或创建时间或Id倒序排列
		/// </summary>
		public virtual void OnSort(
			StaticTableSearchRequest request, ref IQueryable<TEntity> query) {
			if (UpdateTimeTypeTrait<TEntity>.HaveUpdateTime) {
				query = query.OrderByDescending(e => ((IHaveUpdateTime)e).UpdateTime);
			} else if (CreateTimeTypeTrait<TEntity>.HaveCreateTime) {
				query = query.OrderByDescending(e => ((IHaveCreateTime)e).CreateTime);
			} else {
				query = query.OrderByDescending(e => e.Id);
			}
		}

		/// <summary>
		/// 选择需要的字段
		/// </summary>
		public abstract void OnSelect(
			StaticTableSearchRequest request, IList<EntityToTableRow<TEntity>> pairs);
	}
}
