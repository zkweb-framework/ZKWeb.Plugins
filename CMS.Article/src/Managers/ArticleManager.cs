using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Cache;
using ZKWeb.Cache.Interfaces;
using ZKWeb.Plugins.CMS.Article.src.Config;
using ZKWeb.Plugins.CMS.Article.src.Model;
using ZKWeb.Plugins.CMS.Article.src.StaticTableCallbacks;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Server;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.CMS.Article.src.Managers {
	/// <summary>
	/// 文章管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ArticleManager : ICacheCleaner {
		/// <summary>
		/// 文章信息的缓存时间
		/// 默认是15秒，可通过网站配置指定
		/// </summary>
		public TimeSpan ArticleApiInfoCacheTime { get; set; }
		/// <summary>
		/// 文章信息的缓存
		/// </summary>
		protected IsolatedMemoryCache<long, object> ArticleApiInfoCache { get; set; }
		/// <summary>
		/// 文章搜索结果的缓存时间
		/// 默认是15秒，可通过网站配置指定
		/// </summary>
		public TimeSpan ArticleSearchResultCacheTime { get; set; }
		/// <summary>
		/// 文章搜索结果的缓存
		/// </summary>
		protected IsolatedMemoryCache<string, StaticTableSearchResponse> ArticleSearchResultCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ArticleManager() {
			var configManager = Application.Ioc.Resolve<ConfigManager>();
			ArticleApiInfoCacheTime = TimeSpan.FromSeconds(
				configManager.WebsiteConfig.Extra.GetOrDefault(ExtraConfigKeys.ArticleApiInfoCacheTime, 15));
			ArticleApiInfoCache = new IsolatedMemoryCache<long, object>("Ident", "Locale");
			ArticleSearchResultCacheTime = TimeSpan.FromSeconds(
				configManager.WebsiteConfig.Extra.GetOrDefault(ExtraConfigKeys.ArticleSearchResultCacheTime, 15));
			ArticleSearchResultCache = new IsolatedMemoryCache<string, StaticTableSearchResponse>("Ident", "Locale");
		}

		/// <summary>
		/// 获取文章信息
		/// 结果会按文章Id和当前登录用户缓存一定时间
		/// </summary>
		/// <param name="articleId">文章Id</param>
		/// <returns></returns>
		public virtual object GetArticleApiInfo(long articleId) {
			// 从缓存中获取
			var info = ArticleApiInfoCache.GetOrDefault(articleId);
			if (info != null) {
				return info;
			}
			// 从数据库中获取
			UnitOfWork.ReadData<Database.Article>(r => {
				var article = r.Get(a => a.Id == articleId && !a.Deleted);
				if (article == null) {
					return;
				}
				var author = article.Author;
				var classes = article.Classes.Select(c => new { id = c.Id, name = c.Name }).ToList();
				var tags = article.Tags.Select(t => new { id = t.Id, name = t.Name }).ToList();
				var keywords = classes.Select(c => c.name).Concat(tags.Select(t => t.name)).ToList();
				info = new {
					id = article.Id,
					title = article.Title,
					summary = article.Summary,
					contents = article.Contents,
					authorId = author == null ? null : (long?)author.Id,
					authorName = author == null ? null : author.Username,
					classes,
					tags,
					keywords,
					createTime = article.CreateTime,
					lastUpdated = article.LastUpdated
				};
				// 保存到缓存中
				ArticleApiInfoCache.Put(articleId, info, ArticleApiInfoCacheTime);
			});
			return info;
		}

		/// <summary>
		/// 根据当前http请求获取搜索结果
		/// 结果会按请求参数和当前登录用户缓存一定时间
		/// </summary>
		/// <returns></returns>
		public virtual StaticTableSearchResponse GetArticleSearchResponseFromHttpRequest() {
			// 从缓存获取
			var request = HttpContextUtils.CurrentContext.Request;
			var key = request.Url.PathAndQuery;
			var searchResponse = ArticleSearchResultCache.GetOrDefault(key);
			if (searchResponse != null) {
				return searchResponse;
			}
			// 从数据库获取
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var articleListSettings = configManager.GetData<ArticleListSettings>();
			var searchRequest = StaticTableSearchRequest.FromHttpRequest(
				articleListSettings.ArticlesPerPage);
			var callbacks = new ArticleTableCallback().WithExtensions();
			searchResponse = searchRequest.BuildResponseFromDatabase(callbacks);
			// 保存到缓存中并返回
			ArticleSearchResultCache.Put(key, searchResponse, ArticleSearchResultCacheTime);
			return searchResponse;
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
