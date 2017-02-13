using ZKWebStandard.Web;
using ZKWebStandard.Web.Wrappers;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.HttpContextWrappers {
	/// <summary>
	/// 可视化编辑使用的Http请求
	/// 用于重载路径相关的成员
	/// </summary>
	internal class VisualEditorHttpRequest : HttpRequestWrapper {
		public string OriginalPath { get; protected set; }
		public override string Path { get { return OriginalPath; } }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="originalRequest">原始的http请求</param>
		/// <param name="path">原始的请求路径</param>
		public VisualEditorHttpRequest(
			IHttpRequest originalRequest, string path) :
			base(originalRequest) {
			OriginalPath = path;
		}
	}
}
