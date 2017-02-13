using ZKWebStandard.Web;
using ZKWebStandard.Web.Wrappers;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.HttpContextWrappers {
	/// <summary>
	/// 可视化编辑使用的Http上下文
	/// </summary>
	internal class VisualEditorHttpContext : HttpContextWrapper {
		public VisualEditorHttpRequest ChildRequest { get; protected set; }
		public VisualEditorHttpResponse ChildResponse { get; protected set; }
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
		/// <param name="path">原始的请求路径</param>
		public VisualEditorHttpContext(
			IHttpContext originalContext, string path) :
			base(originalContext) {
			ChildRequest = new VisualEditorHttpRequest(originalContext.Request, path);
			ChildResponse = new VisualEditorHttpResponse(originalContext.Response, this);
		}
	}
}
