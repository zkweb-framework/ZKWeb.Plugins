using ZKWeb.Plugins.CMS.ImageBrowser.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.Article.src.ImageBrowsers {
	/// <summary>
	/// 文章图片浏览器
	/// </summary>
	[ExportMany]
	public class ArticleImageBrowser : ImageBrowserBuilder {
		public override string Category { get { return "Article"; } }
	}
}
