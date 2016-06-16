using DotLiquid;
using System;
using System.IO;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Base.src.TemplateTags {
	/// <summary>
	/// 在当前位置引用js文件
	/// </summary>
	/// <example>
	/// {% include_js_here "/static/common.base.js/test.js" %}
	/// {% include_js_here variable %}
	/// </example>
	public class IncludeJsHere : Tag {
		/// <summary>
		/// 描画引用标签
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			var path = (context[Markup.Trim()] ?? "").ToString();
			if (string.IsNullOrEmpty(path)) {
				throw new NullReferenceException("js path can't be empty");
			}
			result.Write(string.Format(
				"<script src='{0}' type='text/javascript'></script>",
				HttpUtils.HtmlEncode(path)));
		}
	}
}
