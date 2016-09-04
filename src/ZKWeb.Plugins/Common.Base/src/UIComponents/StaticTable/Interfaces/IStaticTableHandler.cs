using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Interfaces {
	/// <summary>
	/// 静态表格处理器
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	public interface IStaticTableHandler<TEntity, TPrimaryKey>
		where TEntity : class, IEntity<TPrimaryKey> {
		/// <summary>
		/// 包装查询函数
		/// 可以在这里控制使用的过滤器等
		/// </summary>
		/// <param name="request">搜索请求</param>
		/// <param name="queryMethod">查询函数</param>
		/// <returns></returns>
		Func<TResult> WrapQueryMethod<TResult>(
			StaticTableSearchRequest request, Func<TResult> queryMethod);

		/// <summary>
		/// 过滤数据
		/// </summary>
		/// <param name="request">搜索请求</param>
		/// <param name="context">数据库上下文</param>
		/// <param name="query">查询对象</param>
		void OnQuery(StaticTableSearchRequest request, ref IQueryable<TEntity> query);

		/// <summary>
		/// 排序数据
		/// </summary>
		/// <param name="request">搜索请求</param>
		/// <param name="context">数据库上下文</param>
		/// <param name="query">查询对象</param>
		void OnSort(StaticTableSearchRequest request, ref IQueryable<TEntity> query);

		/// <summary>
		/// 选择需要的字段
		/// 这个函数不传入数据库上下文，因为仍未分页原来的数据库上下文不能用于查询其他数据
		/// </summary>
		/// <param name="request">搜索请求</param>
		/// <param name="pairs">对象和需要字段的组合列表</param>
		void OnSelect(StaticTableSearchRequest request, IList<EntityToTableRow<TEntity>> pairs);
	}
}
