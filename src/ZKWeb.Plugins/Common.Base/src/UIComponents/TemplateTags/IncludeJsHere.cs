using DotLiquid;
using System;
using System.IO;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.TemplateTags {
	/// <summary>
	/// 在当前位置引用js文件
	/// </summary>
	/// <example>
	/// {% include_js_here "/static/common.base.js/test.js" %}
	/// {% include_js_here variable %}
	/// </example>
	public class IncludeJsHere : Tag {
		/// <summary>
		/// 引用Js的Html格式
		/// </summary>
		public const string JsHtmlFormat = "<script src='{0}' type='text/javascript'></script>";

		/// <summary>
		/// 描画引用标签
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			var path = (context[Markup.Trim()] ?? "").ToString();
			if (string.IsNullOrEmpty(path)) {
				throw new ArgumentNullException("js path can't be empty");
			}
			result.Write(string.Format(JsHtmlFormat, HttpUtils.HtmlEncode(path)));
		}
	}
}
