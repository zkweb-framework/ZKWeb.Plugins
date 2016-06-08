using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Collections;

namespace ZKWeb.Plugins.Common.Base.src.TemplateTags {
	/// <summary>
	/// 设置页面关键词
	/// 需要配合"render_metadata"标签使用
	/// 例子
	/// {% use_meta_keywords "keywords" %}
	/// {% use_meta_keywords variable %}
	/// </summary>
	public class UseMetaKeywords : Tag {
		/// <summary>
		/// 设置页面关键词
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			var metadata = (context[RenderMetadata.Key] ?? "").ToString();
			var keywords = context[Markup.Trim()] ?? "";
			var keywordsString = (keywords is IEnumerable) ?
				string.Join(",", (keywords as IEnumerable).OfType<object>()) :
				keywords.ToString();
			metadata += string.Format(
				"<meta name='keywords' content='{0}' />\r\n",
				HttpUtility.HtmlAttributeEncode(keywordsString));
			context.Environments[0][RenderMetadata.Key] = metadata; // 设置到顶级空间
		}
	}
}
