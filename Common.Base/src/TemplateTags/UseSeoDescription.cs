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
	/// 设置SEO使用的页面描述
	/// 例子
	/// {% use_seo_description "description" %}
	/// {% use_seo_description variable %}
	/// </summary>
	public class UseSeoDescription : Tag {
		/// <summary>
		/// 设置SEO使用的页面描述
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			var metadata = (context[RenderMetadata.Key] ?? "").ToString();
			var description = (context[Markup.Trim()] ?? "").ToString();
			metadata += string.Format(
				"<meta name='description' content='{0}' />",
				HttpUtility.HtmlAttributeEncode(description));
			context.Environments[0][RenderMetadata.Key] = metadata; // 设置到顶级空间
		}
	}
}
