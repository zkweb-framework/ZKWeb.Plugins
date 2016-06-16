using System.Collections.Generic;
using System.Linq;
using ZKWeb.Database;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 静态表格回调
	/// </summary>
	/// <typeparam name="T">数据类型</typeparam>
	public interface IStaticTableCallback<T> {
		/// <summary>
		/// 过滤数据
		/// </summary>
		/// <param name="request">搜索请求</param>
		/// <param name="context">数据库上下文</param>
		/// <param name="query">查询对象</param>
		void OnQuery(StaticTableSearchRequest request, DatabaseContext context, ref IQueryable<T> query);

		/// <summary>
		/// 排序数据
		/// </summary>
		/// <param name="request">搜索请求</param>
		/// <param name="context">数据库上下文</param>
		/// <param name="query">查询对象</param>
		void OnSort(StaticTableSearchRequest request, DatabaseContext context, ref IQueryable<T> query);

		/// <summary>
		/// 选择需要的字段
		/// 这个函数不传入数据库上下文，因为仍未分页原来的数据库上下文不能用于查询其他数据
		/// </summary>
		/// <param name="request">搜索请求</param>
		/// <param name="pairs">对象和需要字段的组合列表</param>
		void OnSelect(StaticTableSearchRequest request, List<EntityToTableRow<T>> pairs);
	}
}
