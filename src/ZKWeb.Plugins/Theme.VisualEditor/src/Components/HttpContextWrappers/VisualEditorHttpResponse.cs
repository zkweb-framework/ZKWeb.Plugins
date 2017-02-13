using ZKWebStandard.Web;
using ZKWebStandard.Web.Wrappers;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.HttpContextWrappers {
	/// <summary>
	/// 可视化编辑使用的Http回应
	/// </summary>
	internal class VisualEditorHttpResponse : HttpResponseWrapper {
		public VisualEditorHttpContext ParentContext { get; protected set; }
		public override IHttpContext HttpContext {
			get { return ParentContext; }
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="originalResponse">原始的回应</param>
		/// <param name="parentContext">所属的Http上下文</param>
		public VisualEditorHttpResponse(
			IHttpResponse originalResponse,
			VisualEditorHttpContext parentContext) :
			base(originalResponse) {
			ParentContext = parentContext;
		}
	}
}
