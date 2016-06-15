using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWebStandard.Ioc;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web;
using ZKWeb.Web.Abstractions;

namespace ZKWeb.Plugins.CMS.Article.src.Controllers {
	/// <summary>
	/// 文章控制器
	/// </summary>
	[ExportMany]
	public class ArticleController : IController {
		/// <summary>
		/// 前台文章详情页
		/// </summary>
		/// <returns></returns>
		[Action("article/view")]
		public IActionResult View() {
			return new TemplateResult("cms.article/article_view.html");
		}

		/// <summary>
		/// 前台文章列表页
		/// </summary>
		/// <returns></returns>
		[Action("article/list")]
		public IActionResult List() {
			return new TemplateResult("cms.article/article_list.html");
		}
	}
}
