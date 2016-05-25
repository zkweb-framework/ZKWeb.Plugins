using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DotLiquid.Tags;

namespace ZKWeb.Plugins.Common.Base.src.TemplateTags {
	/// <summary>
	/// 根据Url分页的控件
	/// 需要配合Model.Pagination使用
	/// 例子
	/// {% url_pagination response.Pagination %}
	/// </summary>
	public class UrlPagination : Tag {
		/// <summary>
		/// 设置到url时使用的参数名
		/// </summary>
		public const string UrlParam = "page";

		/// <summary>
		/// 描画Html到模板
		/// </summary>
		public override void Render(Context context, TextWriter writer) {
			var pagination = context[Markup.Trim()];
			if (pagination == null) {
				throw new ArgumentNullException("pagination is null");
			}
			var scope = Hash.FromAnonymousObject(new { pagination, urlParam = UrlParam });
			context.Stack(scope, () => {
				var includeTag = new Include();
				includeTag.Initialize("include", "common.base/tmpl.url_pagination.html", null);
				includeTag.Render(context, writer);
			});
		}
	}
}
