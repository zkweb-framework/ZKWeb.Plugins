using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// Ajax表格数据的搜索请求
	/// </summary>
	public class AjaxTableSearchRequest {
		/// <summary>
		/// 请求的页面序号
		/// </summary>
		public int PageIndex { get; set; }
		/// <summary>
		/// 每页显示数量
		/// </summary>
		public int PageSize { get; set; }
		/// <summary>
		/// 搜索关键字
		/// </summary>
		public string Keyword { get; set; }
		/// <summary>
		/// 搜索条件
		/// </summary>
		public IDictionary<string, object> Conditions { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public AjaxTableSearchRequest() {
			Conditions = new Dictionary<string, object>();
		}
	}
}
