using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Plugins.Common.Base.src.TemplateTags;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 静态表格数据的搜索请求
	/// </summary>
	public class StaticTableSearchRequest : BaseTableSearchRequest {
		/// <summary>
		/// 获取每页数量使用的键值
		/// </summary>
		public const string PageSizeKey = "page_size";
		/// <summary>
		/// 获取关键字使用的键值
		/// </summary>
		public const string KeywordKey = "keyword";

		/// <summary>
		/// 把http请求转换到搜索请求
		/// </summary>
		/// <param name="request">http请求</param>
		/// <param name="defaultPageSize">默认的每页数量，默认是50</param>
		/// <returns></returns>
		public static StaticTableSearchRequest FromHttpRequest(
			HttpRequestBase request, int? defaultPageSize = null) {
			var searchRequest = new StaticTableSearchRequest();
			var pageNo = request.Get<string>(UrlPagination.UrlParam, null);
			searchRequest.PageNo = ((pageNo == UrlPagination.LastPageAlias) ?
				UrlPagination.LastPageNo : pageNo.ConvertOrDefault<int>(1));
			searchRequest.PageSize = request.Get(PageSizeKey, defaultPageSize ?? 50);
			searchRequest.Keyword = request.Get<string>(KeywordKey);
			foreach (var pair in request.GetAll()) {
				searchRequest.Conditions[pair.Key] = pair.Value;
			}
			return searchRequest;
		}

		/// <summary>
		/// 把当前http请求转换到搜索请求
		/// </summary>
		/// <param name="defaultPageSize">默认的每页数量，默认是50</param>
		/// <returns></returns>
		public static StaticTableSearchRequest FromHttpRequest(int? defaultPageSize = null) {
			return FromHttpRequest(HttpContextUtils.CurrentContext.Request, defaultPageSize);
		}
	}
}
