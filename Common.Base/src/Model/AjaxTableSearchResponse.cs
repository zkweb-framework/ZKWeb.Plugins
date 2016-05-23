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
		/// 返回的页面序号，从0开始
		/// </summary>
		public int PageIndex { get; set; }
		/// <summary>
		/// 每页显示数量
		/// </summary>
		public int PageSize { get; set; }
		/// <summary>
		/// 分页信息
		/// </summary>
		public Pagination Pagination { get; set; }
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
			Pagination = new Pagination();
			Rows = new List<Dictionary<string, object>>();
			Columns = new List<AjaxTableColumn>();
		}
	}
}
