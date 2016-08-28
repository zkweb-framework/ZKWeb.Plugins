using ZKWeb.Web;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.Controllers {
	/// <summary>
	/// 控制器的基础类
	/// </summary>
	public abstract class ControllerBase : IController {
		/// <summary>
		/// 当前的Http上下文
		/// </summary>
		public virtual IHttpContext Context { get { return HttpManager.CurrentContext; } }
		/// <summary>
		/// 当前的Http请求
		/// </summary>
		public virtual IHttpRequest Request { get { return Context.Request; } }
		/// <summary>
		/// 当前的Http回应
		/// </summary>
		public virtual IHttpResponse Response { get { return Context.Response; } }
	}
}
