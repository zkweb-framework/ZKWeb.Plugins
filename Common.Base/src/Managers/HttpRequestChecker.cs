using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Base.src.Managers {
	/// <summary>
	/// Http请求的检查器
	/// </summary>
	public static class HttpRequestChecker {
		/// <summary>
		/// 要求当前请求是ajax请求
		/// 非ajax请求时抛出403例外，通常用于防止跨站攻击
		/// </summary>
		/// <param name="errorMessage">错误信息，没有时使用默认信息</param>
		public static void RequieAjaxRequest(string errorMessage = null) {
			var request = HttpContextUtils.CurrentContext.Request;
			if (!request.IsAjaxRequest()) {
				throw new HttpException(403, errorMessage ?? "Request required to be ajax request");
			}
		}
	}
}
