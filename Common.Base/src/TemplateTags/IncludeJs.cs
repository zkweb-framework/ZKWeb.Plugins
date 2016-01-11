using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.TemplateTags {
	/// <summary>
	/// 引用javascript文件
	/// 必须使用在引用footer.html之前
	/// 例子
	/// {% include_js "/static/common.base.js/test.js" %}
	/// {% include_js variable %}
	/// </summary>
	public class IncludeJs : Tag {
		/// <summary>
		/// 变量名
		/// </summary>
		public const string Key = "__js";

		/// <summary>
		/// 添加html到变量中，不重复添加
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			var js = (context[Key] ?? "").ToString();
			var path = (context[Markup.Trim()] ?? "").ToString();
			if (string.IsNullOrEmpty(path)) {
				throw new NullReferenceException("js path can't be empty");
			}
			var html = string.Format(
				"<script src='{0}' type='text/javascript'></script>\r\n",
				HttpUtility.HtmlAttributeEncode(path));
			if (!js.Contains(html)) {
				js += html;
				context.Environments[0][Key] = js; // 设置到顶级空间
			}
		}
	}
}
