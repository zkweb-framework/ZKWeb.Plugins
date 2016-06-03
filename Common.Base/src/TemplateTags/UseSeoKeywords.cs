using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace ZKWeb.Plugins.Common.Base.src.TemplateTags {
	/// <summary>
	/// 设置SEO使用的页面关键词
	/// 例子
	/// {% use_seo_keywords "keywords" %}
	/// {% use_seo_keywords variable %}
	/// </summary>
	public class UseSeoKeywords : Tag {
		/// <summary>
		/// 设置SEO使用的页面关键词
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			var metadata = (context[RenderMetadata.Key] ?? "").ToString();
			var keywords = (context[Markup.Trim()] ?? "").ToString();
			metadata += string.Format(
				"<meta name='keywords' content='{0}' />",
				HttpUtility.HtmlAttributeEncode(keywords));
			context.Environments[0][RenderMetadata.Key] = metadata; // 设置到顶级空间
		}
	}
}
