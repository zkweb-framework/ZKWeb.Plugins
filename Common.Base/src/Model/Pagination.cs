using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 分页信息
	/// </summary>
	public class Pagination {
		/// <summary>
		/// 可到达的最后一页
		/// </summary>
		public int ReachableLastPage { get; set; }
		/// <summary>
		/// 可到达的最后一页是否真正的最后一页
		/// </summary>
		public bool ReachableLastPageIsLastPage { get; set; }
		/// <summary>
		/// 总数量
		/// 因为影响性能，不一定会获取
		/// </summary>
		public long? TotalCount { get; set; }
		/// <summary>
		/// 分页栏的链接列表
		/// </summary>
		public List<Link> Links { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public Pagination() {
			Links = new List<Link>();
		}

		/// <summary>
		/// 分页栏中的页面链接
		/// </summary>
		public class Link {
			/// <summary>
			/// 页面序号
			/// 如果序号是数字，从0开始
			/// 可能是: ["first", "prev", "0", "ellipsis", "next", "last"]
			/// </summary>
			public string Page { get; set; }
			/// <summary>
			/// 页面名称
			/// 如果序号是数字，从1开始（需要显示序号+1）
			/// 可能是: ["首页", "上一页", "1", "...", "下一页", "末页"]
			/// </summary>
			public string Name { get; set; }
			/// <summary>
			/// 链接状态
			/// 可能是: ["enabled", "disabled", "active", "ellipsis"]
			/// </summary>
			public string State { get; set; }

			/// <summary>
			/// 初始化
			/// </summary>
			public Link() { }

			/// <summary>
			/// 初始化
			/// </summary>
			public Link(string page, string name, string state) {
				Page = page;
				Name = name;
				State = state;
			}
		}
	}
}
