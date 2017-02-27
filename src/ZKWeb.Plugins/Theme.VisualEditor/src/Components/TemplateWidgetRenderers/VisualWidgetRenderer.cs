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
		/// 获取前Html
		/// </summary>
		protected override string GetBeforeHtml(Context context, TemplateWidget widget) {
			var lastScope = context.Scopes.Last();
			var cacheKey = widget.GetCacheKey();
			var style = HttpUtils.HtmlEncode(lastScope["__Style"]);
			var html = $"<div class='template_widget' data-widget='{cacheKey}' style='{style}'>";
			var beforeHtml = lastScope["__BeforeHtml"] as string;
			if (!string.IsNullOrEmpty(beforeHtml)) {
				html += beforeHtml;
			}
			return html;
		}

		/// <summary>
		/// 获取后Html
		/// </summary>
		protected override string GetAfterHtml(Context context, TemplateWidget widget) {
			var lastScope = context.Scopes.Last();
			var html = "</div>";
			var afterHtml = lastScope["__AfterHtml"] as string;
			if (!string.IsNullOrEmpty(afterHtml)) {
				html += afterHtml;
			}
			return html;
		}
	}
}
