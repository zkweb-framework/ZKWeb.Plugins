using ZKWeb.Plugins.Common.GenericClass.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.Article.src.Controllers {
	/// <summary>
	/// 文章分类
	/// </summary>
	[ExportMany]
	public class ArticleClassController :
		GenericClassControllerBase<ArticleClassController> {
		public override string Name { get { return "ArticleClass"; } }
	}
}
