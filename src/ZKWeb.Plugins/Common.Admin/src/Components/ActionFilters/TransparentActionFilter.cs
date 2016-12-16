using System;
using ZKWeb.Web;

namespace ZKWeb.Plugins.Common.Admin.src.Components.ActionFilters {
	/// <summary>
	/// 透明的的Action过滤器
	/// 返回原Action
	/// </summary>
	public class TransparentActionFilter : IActionFilter {
		/// <summary>
		/// 返回原Action
		/// </summary>
		public Func<IActionResult> Filter(Func<IActionResult> action) {
			return action;
		}
	}
}
