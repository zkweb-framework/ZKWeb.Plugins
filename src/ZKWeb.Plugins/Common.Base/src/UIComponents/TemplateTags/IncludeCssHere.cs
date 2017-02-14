using DotLiquid;
using System;
using System.IO;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.TemplateTags {
	/// <summary>
	/// 在当前位置引用css文件
	/// </summary>
	/// <example>
	/// {% include_css_here "/static/common.base.css/test.css" %}
	/// {% include_css_here variable %}
	/// </example>
	public class IncludeCssHere : Tag {
		/// <summary>
		/// 引用Css的Html格式
		/// </summary>
		public const string CssHtmlFormat = "<link href='{0}' rel='stylesheet' type='text/css' />";

		/// <summary>
		/// 描画引用标签
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			var path = (context[Markup.Trim()] ?? "").ToString();
			if (string.IsNullOrEmpty(path)) {
				throw new NullReferenceException("css path can't be empty");
			}
			result.Write(string.Format(CssHtmlFormat, HttpUtils.HtmlEncode(path)));
		}
	}
}
