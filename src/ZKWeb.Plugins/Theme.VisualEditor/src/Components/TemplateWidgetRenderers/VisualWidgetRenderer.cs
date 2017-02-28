using DotLiquid;
using ZKWeb.Templating.DynamicContents;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.TemplateWidgetRenderers {
	/// <summary>
	/// 可视化编辑使用的模板模块渲染器
	/// 支持自定义内嵌样式和前后Html
	/// </summary>
	[ExportMany(ClearExists = true)]
	public class VisualWidgetRenderer : TemplateWidgetRenderer {
		/// <summary>
		/// Css类的参数名
		/// </summary>
		public const string CssClassKey = "__CssClass";
		/// <summary>
		/// 内嵌Css的参数名
		/// </summary>
		public const string InlineCssKey = "__InlineCss";
		/// <summary>
		/// 前置Html的参数名
		/// </summary>
		public const string BeforeHtmlKey = "__BeforeHtml";
		/// <summary>
		/// 后置Html的参数名
		/// </summary>
		public const string AfterHtmlKey = "__AfterHtml";

		/// <summary>
		/// 获取前Html
		/// </summary>
		protected override string GetBeforeHtml(Context context, TemplateWidget widget) {
			var firstScope = context.Scopes[0];
			var cacheKey = widget.GetCacheKey();
			var cssClass = HttpUtils.HtmlEncode(firstScope[CssClassKey]);
			var style = HttpUtils.HtmlEncode(firstScope[InlineCssKey]);
			var html = $"<div class='template_widget {cssClass}' data-widget='{cacheKey}' style='{style}'>";
			var beforeHtml = firstScope[BeforeHtmlKey] as string;
			if (!string.IsNullOrEmpty(beforeHtml)) {
				html += beforeHtml;
			}
			return html;
		}

		/// <summary>
		/// 获取后Html
		/// </summary>
		protected override string GetAfterHtml(Context context, TemplateWidget widget) {
			var firstScope = context.Scopes[0];
			var html = "</div>";
			var afterHtml = firstScope[AfterHtmlKey] as string;
			if (!string.IsNullOrEmpty(afterHtml)) {
				html += afterHtml;
			}
			return html;
		}
	}
}
