using DotLiquid;
using System.Linq;
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
			var newestScope = context.Scopes[0];
			var cacheKey = widget.GetCacheKey();
			var style = HttpUtils.HtmlEncode(newestScope[InlineCssKey]);
			var html = $"<div class='template_widget' data-widget='{cacheKey}' style='{style}'>";
			var beforeHtml = newestScope[BeforeHtmlKey] as string;
			if (!string.IsNullOrEmpty(beforeHtml)) {
				html += beforeHtml;
			}
			return html;
		}

		/// <summary>
		/// 获取后Html
		/// </summary>
		protected override string GetAfterHtml(Context context, TemplateWidget widget) {
			var newestScope = context.Scopes[0];
			var html = "</div>";
			var afterHtml = newestScope[AfterHtmlKey] as string;
			if (!string.IsNullOrEmpty(afterHtml)) {
				html += afterHtml;
			}
			return html;
		}
	}
}
