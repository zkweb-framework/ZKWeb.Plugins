using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.CMS.Article.src.Model {
	/// <summary>
	/// 网站附加配置中使用的键
	/// </summary>
	public static class ExtraConfigKeys {
		/// <summary>
		/// 文章信息的缓存时间，单位是秒
		/// </summary>
		public const string ArticleApiInfoCacheTime = "CMS.Article.ArticleApiInfoCacheTime";
		/// <summary>
		/// 文章搜索结果的缓存时间，单位是秒
		/// </summary>
		public const string ArticleSearchResultCacheTime = "CMS.Article.ArticleSearchResultCacheTime";
	}
}
