using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 表格数据的搜索回应的基础类
	/// </summary>
	public abstract class BaseTableSearchResponse : ILiquidizable {
		/// <summary>
		/// 返回的页面序号，从1开始
		/// </summary>
		public int PageNo { get; set; }
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
		public List<IDictionary<string, object>> Rows { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public BaseTableSearchResponse() {
			Pagination = new Pagination();
			Rows = new List<IDictionary<string, object>>();
		}

		/// <summary>
		/// 允许描画到模板
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return new { PageNo, PageSize, Pagination, Rows };
		}
	}
}
