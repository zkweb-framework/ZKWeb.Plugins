﻿using DryIoc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Repositories;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// Ajax表格数据的搜索回应
	/// </summary>
	public class AjaxTableSearchResponse {
		/// <summary>
		/// 返回的页面序号，从0开始
		/// </summary>
		public int PageIndex { get; set; }
		/// <summary>
		/// 每页显示数量
		/// </summary>
		public int PageSize { get; set; }
		/// <summary>
		/// 当前是否最后一页
		/// </summary>
		public bool IsLastPage { get; set; }
		/// <summary>
		/// 数据列表
		/// </summary>
		public List<Dictionary<string, object>> Rows { get; set; }
		/// <summary>
		/// 表格列列表
		/// </summary>
		public List<AjaxTableColumn> Columns { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public AjaxTableSearchResponse() {
			Rows = new List<Dictionary<string, object>>();
			Columns = new List<AjaxTableColumn>();
		}

		/// <summary>
		/// 从搜索请求和表格回调生成搜索回应
		/// 这个函数主要处理分页和配合表格回调设置结果
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="request">搜索请求</param>
		/// <param name="callbacks">表格回调</param>
		/// <returns></returns>
		public static AjaxTableSearchResponse FromRequest<TData>(
			AjaxTableSearchRequest request, IEnumerable<IAjaxTableCallback<TData>> callbacks)
			where TData : class {
			var response = new AjaxTableSearchResponse();
			UnitOfWork.Read(context => {
				// 从数据库获取数据，过滤并排序
				var query = context.Query<TData>();
				foreach (var callback in callbacks) {
					callback.OnQuery(request, context, ref query);
				}
				foreach (var callback in callbacks) {
					callback.OnSort(request, context, ref query);
				}
				// 分页，数量+1是为了检测是否最后一页
				// 当前页没有任何内容时返回最后一页的数据
				int offset = -1;
				try { offset = checked(request.PageIndex * request.PageSize); } catch { }
				var subQuery = (offset >= 0) ? query.Skip(offset).Take(request.PageSize + 1) : null;
				if (subQuery == null || !subQuery.Any()) {
					int total = query.Count();
					request.PageIndex = (total > 0) ? ((total - 1) / request.PageSize) : 0;
					subQuery = query.Skip(request.PageIndex * request.PageSize).Take(request.PageSize + 1);
				}
				query = subQuery;
				// 获取查询结果（这里会选择所有列，为了扩展性这里实现不了只选择需要的列）
				// 并判断是否最后一页
				var queryResults = query.ToList();
				response.PageIndex = request.PageIndex;
				response.PageSize = request.PageSize;
				if (queryResults.Count > response.PageSize) {
					queryResults.RemoveAt(response.PageSize);
				} else {
					response.IsLastPage = true;
				}
				// 选择数据
				// 默认把对象转换到的字符串保存到ToString中
				var pairs = queryResults
					.Select(r => new KeyValuePair<TData, Dictionary<string, object>>(
						r, new Dictionary<string, object>()))
					.ToList();
				foreach (var pair in pairs) {
					pair.Value["ToString"] = pair.Key.ToString();
				}
				foreach (var callback in callbacks) {
					callback.OnSelect(request, pairs);
				}
				response.Rows = pairs.Select(p => p.Value).ToList();
				// 调用返回搜索回应前的回调，这里会添加需要的列
				foreach (var callback in callbacks) {
					callback.OnResponse(request, response);
				}
			});
			return response;
		}
	}
}
