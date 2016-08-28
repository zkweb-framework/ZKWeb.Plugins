using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces {
	/// <summary>
	/// Ajax表格处理器
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	public interface IAjaxTableHandler<TEntity, TPrimaryKey>
		where TEntity : class, IEntity<TPrimaryKey> {
		/// <summary>
		/// 构建表格时的处理
		/// 这个函数与搜索无关，仅在显示表格和搜索栏时调用
		/// </summary>
		/// <param name="table">表格</param>
		/// <param name="searchBar">搜索栏</param>
		void BuildTable(AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar);

		/// <summary>
		/// 包装查询函数
		/// 可以在这里控制使用的过滤器等
		/// </summary>
		/// <param name="request">搜索请求</param>
		/// <param name="queryMethod">查询函数</param>
		/// <returns></returns>
		Func<TResult> WrapQueryMethod<TResult>(
			AjaxTableSearchRequest request, Func<TResult> queryMethod);

		/// <summary>
		/// 过滤数据
		/// </summary>
		/// <param name="request">搜索请求</param>
		/// <param name="context">数据库上下文</param>
		/// <param name="query">查询对象</param>
		void OnQuery(AjaxTableSearchRequest request, ref IQueryable<TEntity> query);

		/// <summary>
		/// 排序数据
		/// </summary>
		/// <param name="request">搜索请求</param>
		/// <param name="context">数据库上下文</param>
		/// <param name="query">查询对象</param>
		void OnSort(AjaxTableSearchRequest request, ref IQueryable<TEntity> query);

		/// <summary>
		/// 选择需要的字段
		/// 这个函数不传入数据库上下文，因为仍未分页原来的数据库上下文不能用于查询其他数据
		/// </summary>
		/// <param name="request">搜索请求</param>
		/// <param name="pairs">对象和需要字段的组合列表</param>
		void OnSelect(AjaxTableSearchRequest request, IList<EntityToTableRow<TEntity>> pairs);

		/// <summary>
		/// 添加列和批量处理
		/// </summary>
		/// <param name="request">搜索请求</param>
		/// <param name="response">搜索回应</param>
		void OnResponse(AjaxTableSearchRequest request, AjaxTableSearchResponse response);
	}
}
