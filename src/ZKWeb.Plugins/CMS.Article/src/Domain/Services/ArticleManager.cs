using System;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.Plugins.CMS.Article.src.Components.ExtraConfigKeys;
using ZKWeb.Plugins.CMS.Article.src.Components.GenericConfigs;
using ZKWeb.Plugins.CMS.Article.src.UIComponents.StaticTableHandlers;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Extensions;
using ZKWeb.Server;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.Article.src.Domain.Services {
	/// <summary>
	/// 文章管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ArticleManager :
		DomainServiceBase<Entities.Article, Guid>, ICacheCleaner {
		/// <summary>
		/// 文章信息的缓存时间
		/// 默认是15秒，可通过网站配置指定
		/// </summary>
		public TimeSpan ArticleApiInfoCacheTime { get; set; }
		/// <summary>
		/// 文章信息的缓存
		/// </summary>
		protected IsolatedMemoryCache<Guid, object> ArticleApiInfoCache { get; set; }
		/// <summary>
		/// 文章搜索结果的缓存时间
		/// 默认是15秒，可通过网站配置指定
		/// </summary>
		public TimeSpan ArticleSearchResultCacheTime { get; set; }
		/// <summary>
		/// 文章搜索结果的缓存
		/// </summary>
		protected IsolatedMemoryCache<int, StaticTableSearchResponse> ArticleSearchResultCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ArticleManager() {
			var configManager = Application.Ioc.Resolve<ConfigManager>();
			var extra = configManager.WebsiteConfig.Extra;
			ArticleApiInfoCacheTime = TimeSpan.FromSeconds(extra.GetOrDefault(
				ArticleExtraConfigKeys.ArticleApiInfoCacheTime, 15));
			ArticleApiInfoCache = new IsolatedMemoryCache<Guid, object>("Ident", "Locale");
			ArticleSearchResultCacheTime = TimeSpan.FromSeconds(extra.GetOrDefault(
				ArticleExtraConfigKeys.ArticleSearchResultCacheTime, 15));
			ArticleSearchResultCache = (
				new IsolatedMemoryCache<int, StaticTableSearchResponse>("Ident", "Locale", "Url"));
		}

		/// <summary>
		/// 获取文章信息
		/// 结果会按文章Id和当前登录用户缓存一定时间
		/// </summary>
		/// <param name="articleId">文章Id</param>
		/// <returns></returns>
		public virtual object GetArticleApiInfo(Guid articleId) {
			return ArticleApiInfoCache.GetOrCreate(articleId, () => {
				using (UnitOfWork.Scope()) {
					var article = Get(articleId);
					if (article == null) {
						return null;
					}
					var author = article.Author;
					var classes = article.Classes.Select(c => new { id = c.Id, name = c.Name }).ToList();
					var tags = article.Tags.Select(t => new { id = t.Id, name = t.Name }).ToList();
					var keywords = classes.Select(c => c.name).Concat(tags.Select(t => t.name)).ToList();
					return new {
						id = article.Id,
						title = article.Title,
						summary = article.Summary,
						contents = article.Contents,
						authorId = author == null ? null : (Guid?)author.Id,
						authorName = author == null ? null : author.Username,
						classes,
						tags,
						keywords,
						createTime = article.CreateTime,
						UpdateTime = article.UpdateTime
					};
				}
			}, ArticleApiInfoCacheTime);
		}

		/// <summary>
		/// 根据当前http请求获取搜索结果
		/// 结果会按请求参数和当前登录用户缓存一定时间
		/// </summary>
		/// <returns></returns>
		public virtual StaticTableSearchResponse GetArticleSearchResponseFromHttpRequest() {
			return ArticleSearchResultCache.GetOrCreate(0, () => {
				using (UnitOfWork.Scope()) {
					var configManager = Application.Ioc.Resolve<GenericConfigManager>();
					var articleListSettings = configManager.GetData<ArticleListSettings>();
					var searchRequest = StaticTableSearchRequest.FromHttpRequest(
						articleListSettings.ArticlesPerPage);
					var handlers = new ArticleTableHandler().WithExtensions();
					return searchRequest.BuildResponse(handlers);
				}
			}, ArticleSearchResultCacheTime);
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public void ClearCache() {
			ArticleApiInfoCache.Clear();
			ArticleSearchResultCache.Clear();
		}
	}
}
