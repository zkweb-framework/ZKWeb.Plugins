using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 表格数据的搜索请求的基础类
	/// </summary>
	public abstract class BaseTableSearchRequest {
		/// <summary>
		/// 最大允许的每页显示数量
		/// </summary>
		public static int MaxPageSize = 10000;
		/// <summary>
		/// 请求的页面序号，从1开始
		/// </summary>
		public int PageNo { get; set; }
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
		public BaseTableSearchRequest() {
			Conditions = new Dictionary<string, object>();
		}
	}
}
