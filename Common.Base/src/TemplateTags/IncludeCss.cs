using DotLiquid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.TemplateTags {
	/// <summary>
	/// 引用css文件
	/// 必须使用在引用header.html之前
	/// 例子
	/// {% include_css "/static/common.base.css/test.css" %}
	/// {% include_css variable %}
	/// </summary>
	public class IncludeCss : Tag {
		/// <summary>
		/// 变量名
		/// </summary>
		public const string Key = "__css";

		/// <summary>
		/// 添加html到变量中，不重复添加
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			var css = (context[Key] ?? "").ToString();
			var path = (context[Markup.Trim()] ?? "").ToString();
			if (string.IsNullOrEmpty(path)) {
				throw new NullReferenceException("css path can't be empty");
			}
			var html = string.Format(
				"<link href='{0}' rel='stylesheet' type='text/css' />\r\n",
				HttpUtility.HtmlAttributeEncode(path));
			if (!css.Contains(html)) {
				css += html;
				context.Environments[0][Key] = css; // 设置到顶级空间
			}
		}
	}
}
