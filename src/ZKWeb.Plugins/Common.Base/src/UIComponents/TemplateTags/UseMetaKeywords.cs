using DotLiquid;
using System.Collections.Generic;
using System.IO;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.TemplateTags {
	/// <summary>
	/// 设置页面关键词
	/// 需要配合"render_meta_keywords"标签使用
	/// </summary>
	/// <example>
	/// {% use_meta_keywords "keywords" %}
	/// {% use_meta_keywords variable %}
	/// </example>
	public class UseMetaKeywords : Tag {
		/// <summary>
		/// 设置页面关键词
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			var keywords = context[Markup.Trim()] ?? "";
			if (keywords is IEnumerable<string>) {
				// 支持传入列表
				keywords = string.Join(",", (IEnumerable<string>)keywords);
			}
			context.Environments[0][RenderMetaKeywords.Key] = keywords; // 设置到顶级空间
		}
	}
}
