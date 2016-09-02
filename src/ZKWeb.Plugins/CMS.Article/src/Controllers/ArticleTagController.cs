using ZKWeb.Plugins.Common.GenericTag.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.Article.src.Controllers {
	/// <summary>
	/// 文章标签
	/// </summary>
	[ExportMany]
	public class ArticleTagController :
		GenericTagControllerBase<ArticleTagController> {
		public override string Name { get { return "ArticleTag"; } }
	}
}
