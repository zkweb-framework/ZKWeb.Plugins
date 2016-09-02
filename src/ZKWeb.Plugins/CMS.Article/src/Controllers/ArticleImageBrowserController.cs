using ZKWeb.Plugins.CMS.ImageBrowser.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.Article.src.Controllers {
	/// <summary>
	/// 文章图片浏览器
	/// </summary>
	[ExportMany]
	public class ArticleImageBrowserController : ImageBrowserControllerBase {
		public override string Category { get { return "Article"; } }
	}
}
