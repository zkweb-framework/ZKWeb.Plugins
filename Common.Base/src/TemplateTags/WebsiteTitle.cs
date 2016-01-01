using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ZKWeb.Plugins.Common.Base.src.TemplateFilters;

namespace ZKWeb.Plugins.Common.Base.src.TemplateTags {
	/// <summary>
	/// 设置网站标题
	/// 必须使用在引用header.html之前
	/// 例子
	/// {% website_title "Plain Text Title" %}
	/// {% website_title variable_title %}
	/// </summary>
	public class WebsiteTitle : Tag {
		/// <summary>
		/// 变量名
		/// </summary>
		public const string Key = "__title";

		/// <summary>
		/// 设置标题到变量中
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			context[Key] = Filters.WebsiteTitle(context[Markup.Trim()] as string);
		}
	}
}
