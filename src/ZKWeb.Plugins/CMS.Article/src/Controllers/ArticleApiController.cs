using System;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web;
using ZKWeb.Plugins.Common.GenericClass.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.CMS.Article.src.Domain.Services;

namespace ZKWeb.Plugins.CMS.Article.src.Controllers {
	/// <summary>
	/// Api控制器
	/// </summary>
	[ExportMany]
	public class ArticleApiController : ControllerBase {
		/// <summary>
		/// 获取文章分类树
		/// </summary>
		/// <returns></returns>
		[Action("api/article/class_tree", HttpMethods.POST)]
		public IActionResult ArticleClassTree() {
			var classManager = Application.Ioc.Resolve<GenericClassManager>();
			var classTree = classManager.GetTreeWithCache(new ArticleClassController().Type);
			var tree = TreeUtils.Transform(classTree, c => c == null ? null : new { c.Id, c.Name });
			return new JsonResult(new { tree });
		}

		/// <summary>
		/// 获取指定文章分类的信息
		/// </summary>
		/// <returns></returns>
		[Action("api/article/class_info", HttpMethods.POST)]
		public IActionResult ArticleClassName() {
			var classId = Request.Get<Guid>("class");
			var classManager = Application.Ioc.Resolve<GenericClassManager>();
			var classObj = classManager.GetWithCache(classId);
			if (classObj == null || classObj.Type != new ArticleClassController().Type) {
				return new JsonResult(null);
			}
			return new JsonResult(new { name = classObj.Name });
		}

		/// <summary>
		/// 获取指定文章的信息
		/// </summary>
		/// <returns></returns>
		[Action("api/article/info", HttpMethods.POST)]
		public IActionResult ArticleInfo() {
			var articleId = Request.Get<Guid>("id");
			var articleManager = Application.Ioc.Resolve<ArticleManager>();
			var info = articleManager.GetArticleApiInfo(articleId);
			return new JsonResult(info);
		}

		/// <summary>
		/// 搜索文章列表
		/// </summary>
		/// <returns></returns>
		[Action("api/article/search", HttpMethods.POST)]
		public IActionResult ArticleSearch() {
			var articleManager = Application.Ioc.Resolve<ArticleManager>();
			var response = articleManager.GetArticleSearchResponseFromHttpRequest();
			return new JsonResult(new { response });
		}
	}
}
