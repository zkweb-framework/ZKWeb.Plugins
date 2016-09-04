using ZKWeb.Plugins.Common.Base.src.Components.GenericConfigs.Attributes;

namespace ZKWeb.Plugins.CMS.Article.src.Components.GenericConfigs {
	/// <summary>
	/// 文章列表设置
	/// </summary>
	[GenericConfig("CMS.Article.ArticleListSettings", CacheTime = 15)]
	public class ArticleListSettings {
		/// <summary>
		/// 每页显示的文章数量
		/// </summary>
		public int ArticlesPerPage { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ArticleListSettings() {
			ArticlesPerPage = 12;
		}
	}
}
