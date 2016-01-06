using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// Ajax表格数据的搜索回应
	/// </summary>
	public class AjaxTableSearchResponse {
		/// <summary>
		/// 返回的页面序号
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
		public IList<IDictionary<string, object>> Rows { get; set; }
		/// <summary>
		/// 表格列列表
		/// </summary>
		public IList<AjaxTableColumn> Columns { get; set; }

		public AjaxTableSearchResponse() {
			Rows = new List<IDictionary<string, object>>();
			Columns = new List<AjaxTableColumn>();
		}
	}
}
