using ZKWebStandard.Web;
using ZKWebStandard.Web.Wrappers;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.Model {
	/// <summary>
	/// 伪静态使用的Http回应
	/// </summary>
	internal class PesudoStaticHttpResponse : HttpResponseWrapper {
		public PesudoStaticHttpContext ParentContext { get; protected set; }
		public override IHttpContext HttpContext {
			get { return ParentContext; }
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="originalResponse">原始的回应</param>
		/// <param name="parentContext">所属的Http上下文</param>
		public PesudoStaticHttpResponse(
			IHttpResponse originalResponse,
			PesudoStaticHttpContext parentContext) :
			base(originalResponse) {
			ParentContext = parentContext;
		}
	}
}
