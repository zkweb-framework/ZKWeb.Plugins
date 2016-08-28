using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Web;
using ZKWebStandard.Extensions;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.Controllers.Extensions {
	/// <summary>
	/// 控制器的扩展函数
	/// </summary>
	public static class IControllerExtensions {
		/// <summary>
		/// 要求当前请求是ajax请求
		/// 非ajax请求时抛出403错误，通常用于防止跨站攻击
		/// </summary>
		/// <param name="controller">控制器</param>
		/// <param name="errorMessage">错误信息，没有时使用默认信息</param>
		public static void RequireAjaxRequest(
			this IController controller, string errorMessage = null) {
			var request = HttpManager.CurrentContext.Request;
			if (!request.IsAjaxRequest()) {
				throw new ForbiddenException(errorMessage ?? "Request required to be ajax request");
			}
		}
	}
}
