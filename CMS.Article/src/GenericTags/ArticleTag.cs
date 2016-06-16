using ZKWeb.Plugins.Common.GenericTag.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.Article.src.GenericTags {
	/// <summary>
	/// 文章标签
	/// </summary>
	[ExportMany]
	public class ArticleTag : GenericTagBuilder {
		public override string Name { get { return "ArticleTag"; } }
	}
}
