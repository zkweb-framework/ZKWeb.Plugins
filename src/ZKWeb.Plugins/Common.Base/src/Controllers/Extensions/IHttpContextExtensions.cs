using ZKWeb.Plugins.Common.Base.src.UIComponents.TemplateTags;
using ZKWebStandard.Extensions;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.Controllers.Extensions {
	/// <summary>
	/// Http上下文的扩展函数
	/// </summary>
	public static class IHttpContextExtensions {
		/// <summary>
		/// 延迟引用css文件
		/// 仅在控制器返回模板结果并且使用了render_included_css标签时有效
		/// </summary>
		/// <param name="path">css文件路径</param>
		public static void IncludeCssLater(this IHttpContext context, string path) {
			var includedCss = context.GetData<string>(RenderIncludedCss.HttpContextKey) ?? "";
			includedCss += string.Format(
				UIComponents.TemplateTags.IncludeCssLater.CssHtmlFormat, path);
			context.PutData(RenderIncludedCss.HttpContextKey, includedCss);
		}

		/// <summary>
		/// 延迟引用js文件
		/// 仅在控制器返回模板结果并且使用了render_included_js标签时有效
		/// </summary>
		/// <param name="path">js文件路径</param>
		public static void IncludeJsLater(this IHttpContext context, string path) {
			var includedJs = context.GetData<string>(RenderIncludedJs.HttpContextKey) ?? "";
			includedJs += string.Format(
				UIComponents.TemplateTags.IncludeJsLater.JsHtmlFormat, path);
			context.PutData(RenderIncludedJs.HttpContextKey, includedJs);
		}
	}
}
