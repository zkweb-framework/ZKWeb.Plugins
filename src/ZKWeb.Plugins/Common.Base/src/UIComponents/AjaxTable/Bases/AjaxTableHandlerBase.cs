using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.TypeTraits;
using ZKWeb.Plugins.Common.Base.src.Domain.Filters;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Extensions;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Bases {
	/// <summary>
	/// Ajax表格处理器的基础类
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	public abstract class AjaxTableHandlerBase<TEntity, TPrimaryKey> :
		IAjaxTableHandler<TEntity, TPrimaryKey>
		where TEntity : class, IEntity<TPrimaryKey> {
		/// <summary>
		/// 构建表格时的处理
		/// </summary>
		public virtual void BuildTable(
			AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) { }

		/// <summary>
		/// 包装查询函数
		/// 默认根据搜索参数中的Deleted过滤已删除或未删除的对象
		/// </summary>
		public virtual Func<TResult> WrapQueryMethod<TResult>(
			AjaxTableSearchRequest request, Func<TResult> queryMethod) {
			var uow = Application.Ioc.Resolve<IUnitOfWork>();
			var deleted = request.Conditions.GetOrDefault<bool>("Deleted");
			return () => {
				using (uow.DisableQueryFilter(typeof(DeletedFilter)))
				using (uow.EnableQueryFilter(new DeletedFilter(deleted))) {
					return queryMethod();
				}
			};
		}

		/// <summary>
		/// 过滤数据
		/// 默认不过滤数据
		/// </summary>
		public virtual void OnQuery(
			AjaxTableSearchRequest request, ref IQueryable<TEntity> query) { }

		/// <summary>
		/// 排序数据
		/// 默认更新时间或创建时间或Id倒序排列
		/// </summary>
		public virtual void OnSort(
			AjaxTableSearchRequest request, ref IQueryable<TEntity> query) {
			if (CreateTimeTypeTrait<TEntity>.HaveCreateTime) {
				query = query.OrderByDescending(e => ((IHaveCreateTime)e).CreateTime);
			} else {
				query = query.OrderByDescending(e => e.Id);
			}
		}

		/// <summary>
		/// 选择需要的字段
		/// </summary>
		public abstract void OnSelect(
			AjaxTableSearchRequest request, IList<EntityToTableRow<TEntity>> pairs);

		/// <summary>
		/// 添加列和批量处理
		/// </summary>
		public abstract void OnResponse(
			AjaxTableSearchRequest request, AjaxTableSearchResponse response);
	}
}
