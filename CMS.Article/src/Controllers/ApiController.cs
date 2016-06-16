using System;
using ZKWeb.Plugins.CMS.Article.src.GenericClasses;
using ZKWeb.Plugins.CMS.Article.src.Managers;
using ZKWeb.Plugins.Common.GenericClass.src.Manager;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.CMS.Article.src.Controllers {
	/// <summary>
	/// Api控制器
	/// </summary>
	[ExportMany]
	public class ApiController : IController {
		/// <summary>
		/// 获取文章分类树
		/// </summary>
		/// <returns></returns>
		[Action("api/article/class_tree", HttpMethods.POST)]
		public IActionResult ArticleClassTree() {
			var classManager = Application.Ioc.Resolve<GenericClassManager>();
			var classTree = classManager.GetClassTree(new ArticleClass().Type);
			var tree = TreeUtils.Transform(classTree, c => c == null ? null : new { c.Id, c.Name });
			return new JsonResult(new { tree });
		}

		/// <summary>
		/// 获取指定文章分类的信息
		/// </summary>
		/// <returns></returns>
		[Action("api/article/class_info", HttpMethods.POST)]
		public IActionResult ArticleClassName() {
			var classId = HttpManager.CurrentContext.Request.Get<long>("class");
			var classManager = Application.Ioc.Resolve<GenericClassManager>();
			var classObj = classManager.GetClass(classId);
			if (classObj == null || classObj.Type != new ArticleClass().Type) {
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
			var articleId = HttpManager.CurrentContext.Request.Get<long>("id");
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
