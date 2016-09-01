using ZKWebStandard.Web;
using ZKWebStandard.Web.Wrappers;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.Components.PesudoStatic.Wrappers {
	/// <summary>
	/// 伪静态使用的Http上下文
	/// </summary>
	internal class PesudoStaticHttpContext : HttpContextWrapper {
		public PesudoStaticHttpRequest ChildRequest { get; protected set; }
		public PesudoStaticHttpResponse ChildResponse { get; protected set; }
		public override IHttpRequest Request {
			get { return ChildRequest; }
		}
		public override IHttpResponse Response {
			get { return ChildResponse; }
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="originalContext">原始的Http上下文</param>
		/// <param name="path">解析后的请求路径</param>
		/// <param name="queryString">解析后的请求参数</param>
		public PesudoStaticHttpContext(
			IHttpContext originalContext, string path, string queryString) :
			base(originalContext) {
			ChildRequest = new PesudoStaticHttpRequest(originalContext.Request, path, queryString);
			ChildResponse = new PesudoStaticHttpResponse(originalContext.Response, this);
		}
	}
}
