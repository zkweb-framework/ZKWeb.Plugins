using DotLiquid;
using System;
using System.IO;
using DotLiquid.Tags;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.TemplateTags {
	/// <summary>
	/// 根据Url分页的控件
	/// 需要配合Pagination使用
	/// </summary>
	/// <example>
	/// {% url_pagination response.Pagination %}
	/// </example>
	public class UrlPagination : Tag {
		/// <summary>
		/// 设置到url时使用的参数名
		/// </summary>
		public const string UrlParam = "page";
		/// <summary>
		/// 最后一页的别名
		/// </summary>
		public const string LastPageAlias = "last";
		/// <summary>
		/// 最后一页的值
		/// </summary>
		public const int LastPageNo = PaginationExtensions.LastPageNo;

		/// <summary>
		/// 描画Html到模板
		/// </summary>
		public override void Render(Context context, TextWriter writer) {
			var pagination = context[Markup.Trim()];
			if (pagination == null) {
				throw new ArgumentNullException("pagination is null");
			}
			var scope = Hash.FromAnonymousObject(new {
				pagination,
				urlParam = UrlParam,
				lastPageNo = LastPageNo,
				lastPageAlias = LastPageAlias
			});
			context.Stack(scope, () => {
				var includeTag = new Include();
				includeTag.Initialize("include", "common.base/tmpl.url_pagination.html", null);
				includeTag.Render(context, writer);
			});
		}
	}
}
