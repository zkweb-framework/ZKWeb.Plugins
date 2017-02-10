using System;
using ZKWebStandard.Ioc;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using System.ComponentModel;

namespace ZKWeb.Plugins.CMS.Article.src.Controllers {
	/// <summary>
	/// 文章控制器
	/// </summary>
	[ExportMany]
	public class ArticleController : ControllerBase {
		/// <summary>
		/// 前台文章详情页
		/// </summary>
		/// <returns></returns>
		[Action("article/view")]
		[Description("ArticleViewPage")]
		public IActionResult View() {
			return new TemplateResult("cms.article/article_view.html");
		}

		/// <summary>
		/// 前台文章列表页
		/// </summary>
		/// <returns></returns>
		[Action("article/list")]
		[Description("ArticleListPage")]
		public IActionResult List() {
			return new TemplateResult("cms.article/article_list.html");
		}
	}
}
