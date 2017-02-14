using DotLiquid;
using System;
using System.IO;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.TemplateTags {
	/// <summary>
	/// 延迟引用javascript文件
	/// 需要配合"render_included_js"标签使用
	/// 这个标签会影响上下文内容，不应该在有缓存的模板模块中使用
	/// </summary>
	/// <example>
	/// {% include_js_later "/static/common.base.js/test.js" %}
	/// {% include_js_later variable %}
	/// </example>
	public class IncludeJsLater : Tag {
		/// <summary>
		/// 引用Js的Html格式
		/// </summary>
		public const string JsHtmlFormat = IncludeJsHere.JsHtmlFormat + "\r\n";

		/// <summary>
		/// 添加html到变量中，不重复添加
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			var js = (context[RenderIncludedJs.Key] ?? "").ToString();
			var path = (context[Markup.Trim()] ?? "").ToString();
			if (string.IsNullOrEmpty(path)) {
				throw new NullReferenceException("js path can't be empty");
			}
			var html = string.Format(JsHtmlFormat, HttpUtils.HtmlEncode(path));
			if (!js.Contains(html)) {
				js += html;
				context.Environments[0][RenderIncludedJs.Key] = js; // 设置到顶级空间
			}
		}
	}
}
