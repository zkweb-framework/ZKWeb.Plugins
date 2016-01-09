using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// Ajax表格的搜索处理器
	/// </summary>
	public interface IAjaxTableSearchHandler<T> {
		/// <summary>
		/// 获取搜索条件
		/// 这个函数只在构建搜索栏时使用，与搜索无关
		/// </summary>
		/// <returns></returns>
		IEnumerable<FormField> GetConditions();

		/// <summary>
		/// 过滤数据
		/// </summary>
		/// <param name="request">搜索请求</param>
		/// <param name="context">数据库上下文</param>
		/// <param name="query">查询对象</param>
		void OnQuery(AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<T> query);

		/// <summary>
		/// 排序数据
		/// </summary>
		/// <param name="request">搜索请求</param>
		/// <param name="context">数据库上下文</param>
		/// <param name="query">查询对象</param>
		void OnSort(AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<T> query);

		/// <summary>
		/// 选择需要的字段
		/// 这个函数不传入数据库上下文，因为仍未分页原来的数据库上下文不能用于查询其他数据
		/// </summary>
		/// <param name="request">搜索请求</param>
		/// <param name="pairs">对象和需要字段的组合列表</param>
		void OnSelect(AjaxTableSearchRequest request, List<KeyValuePair<T, Dictionary<string, object>>> pairs);

		/// <summary>
		/// 添加列和批量处理
		/// </summary>
		/// <param name="request">搜索请求</param>
		/// <param name="response">搜索回应</param>
		void OnResponse(AjaxTableSearchRequest request, AjaxTableSearchResponse response);
	}
}
