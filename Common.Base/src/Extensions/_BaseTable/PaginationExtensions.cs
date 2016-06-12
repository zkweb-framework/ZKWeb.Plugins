using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.Extensions {
	/// <summary>
	/// 分页信息的扩展函数
	/// </summary>
	public static class PaginationExtensions {
		/// <summary>
		/// Ajax表格搜索请求中，指定分页链接范围时使用的键
		/// </summary>
		public const string AjaxTablePaginationLinkRangeKey = "PaginationLinkRange";
		/// <summary>
		/// Ajax表格搜索请求中，需要获取总数量时使用的键
		/// </summary>
		public const string AjaxTableRequireTotalCountKey = "RequireTotalCount";

		/// <summary>
		/// 更新分页信息中的链接列表
		/// </summary>
		/// <param name="pagination">分页信息</param>
		/// <param name="pageNo">当前页的序号，从1开始</param>
		/// <param name="linkRange">分页链接的范围，范围等于2且当前页等于2时显示1~5</param>
		private static void UpdateLinks(
			this Pagination pagination, int pageNo, int linkRange) {
			pagination.Links.Clear();
			// 首页, 上一页
			bool isFirstPage = pageNo <= 1;
			pagination.Links.Add(new Pagination.Link(
				1, new T("FirstPage"), isFirstPage ? "disabled" : "enabled"));
			pagination.Links.Add(new Pagination.Link(
				isFirstPage ? pageNo : (pageNo - 1),
				new T("PrevPage"), isFirstPage ? "disabled" : "enabled"));
			// 省略号，如果开始页大于1，且有分页范围
			int fromPageNo = (pageNo - linkRange < 1) ? 1 : (pageNo - linkRange);
			if (fromPageNo > 1 && linkRange > 0) {
				pagination.Links.Add(new Pagination.Link(1, "...", "ellipsis"));
			}
			// 指定范围内的分页链接
			for (int i = fromPageNo; i <= pagination.ReachableLastPageNo; ++i) {
				pagination.Links.Add(new Pagination.Link(
					i, i.ToString(), (i == pageNo) ? "active" : "enabled"));
			}
			// 省略号，如果可到达的最后一页不是真正的最后一页，且有分页范围
			if (!pagination.ReachableLastPageIsLastPage && linkRange > 0) {
				pagination.Links.Add(new Pagination.Link(int.MaxValue, "...", "ellipsis"));
			}
			// 下一页，末页
			bool isLastPage = (
				pageNo == pagination.ReachableLastPageNo && pagination.ReachableLastPageIsLastPage);
			pagination.Links.Add(new Pagination.Link(
				isLastPage ? pageNo : (pageNo + 1),
				new T("NextPage"), isLastPage ? "disabled" : "enabled"));
			pagination.Links.Add(new Pagination.Link(
				int.MaxValue, new T("LastPage"), isLastPage ? "disabled" : "enabled"));
		}

		/// <summary>
		/// 对数据进行分页并设置分页信息
		/// 返回分页后的数据
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="pagination">分页信息</param>
		/// <param name="pageNo">页面序号，从1开始</param>
		/// <param name="pageSize">每页数量</param>
		/// <param name="query">需要分页的数据</param>
		/// <param name="linkRange">分页链接的范围，默认等于2, 当前页等于2时显示0~4</param>
		/// <param name="requireTotalCount">是否需要获取总数量，默认等于false，对性能有影响</param>
		/// <returns></returns>
		public static IList<TData> Paging<TData>(
			this Pagination pagination,
			ref int pageNo, int pageSize, IEnumerable<TData> query,
			int? linkRange = null, bool? requireTotalCount = null) {
			// 设置默认参数
			var pageIndex = pageNo - 1;
			if (pageIndex < 0) {
				pageIndex = 0;
			}
			pageSize = (pageSize > 0) ? pageSize : 1;
			linkRange = linkRange ?? 2;
			requireTotalCount = requireTotalCount ?? false;
			// 对数据进行分页
			// 根据分页链接的范围检测后面几页是否有数据，但不需要获取总数量
			// 例: 当前页2, 每页数量50时, skipCount = 100, takeCount = 151
			int skipCount = -1;
			int takeCount = pageSize;
			try { skipCount = checked(pageIndex * pageSize); } catch { }
			try { takeCount = checked(pageSize * (1 + linkRange.Value) + 1); } catch { }
			var subQuery = (skipCount >= 0) ? query.Skip(skipCount).Take(takeCount) : null;
			long? totalCount = null;
			if (subQuery == null || !subQuery.Any()) {
				// 当前页没有任何内容，返回最后一页的数据
				// 目前Linq不支持跳过int.MaxValue以上的数据，所以这里限制了pageIndex的范围
				totalCount = query.LongCount();
				var longPageIndex = (totalCount.Value > 0) ? ((totalCount.Value - 1) / pageSize) : 0;
				var maxIntPageIndex = int.MaxValue / pageSize;
				pageIndex = (int)Math.Min(longPageIndex, maxIntPageIndex);
				subQuery = query.Skip(pageIndex * pageSize).Take(takeCount);
			}
			// 判断可到达的最后一页，和这一页是否真正的最后一页
			// 例: 当前页2, 每页数量50
			//     结果数量151时，可到达的最后一页是4, 不是真正的最后一页
			//     结果数量150时，可到达的最后一页是4, 是真正的最后一页
			//     结果数量50时，可到达的最后一页是2，是真正的最后一页
			int subQueryCount = subQuery.Count();
			var reachableLastPage = (
				pageIndex + ((subQueryCount > 0) ? ((subQueryCount - 1) / pageSize) : 0));
			var reachableLastPageIsLastPage = (
				takeCount == pageSize || takeCount > subQueryCount);
			if (!reachableLastPageIsLastPage) {
				--reachableLastPage;
			}
			// 更新页面序号
			pageNo = pageIndex + 1;
			pagination.ReachableLastPageNo = reachableLastPage + 1;
			pagination.ReachableLastPageIsLastPage = reachableLastPageIsLastPage;
			// 需要时设置总数量
			if (requireTotalCount.Value) {
				totalCount = totalCount ?? query.LongCount();
				pagination.TotalCount = totalCount.Value;
			} else {
				pagination.TotalCount = null;
			}
			// 添加分页链接
			pagination.UpdateLinks(pageNo, linkRange ?? 2);
			// 返回查询结果
			var result = subQuery.Take(pageSize).ToList();
			return result;
		}

		/// <summary>
		/// 对数据进行分页并设置分页信息
		/// 返回分页后的数据
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="pagination">分页信息</param>
		/// <param name="request">表格的搜索请求</param>
		/// <param name="query">需要分页的数据</param>
		/// <returns></returns>
		public static IList<TData> Paging<TData>(
			this Pagination pagination, BaseTableSearchRequest request, IEnumerable<TData> query) {
			int pageIndex = request.PageNo;
			int pageSize = request.PageSize;
			int? linkRange = request.Conditions.GetOrDefault<int?>(AjaxTablePaginationLinkRangeKey);
			bool? requireTotalCount = request.Conditions.GetOrDefault<bool?>(AjaxTableRequireTotalCountKey);
			var result = pagination.Paging(ref pageIndex, pageSize, query, linkRange, requireTotalCount);
			request.PageNo = pageIndex; // 页面序号有可能改变，需要设置回请求中
			return result;
		}
	}
}
