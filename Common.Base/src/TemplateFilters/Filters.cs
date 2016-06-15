using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Config;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Base.src.TemplateFilters {
	/// <summary>
	/// 模板系统的过滤器
	/// </summary>
	public static class Filters {
		/// <summary>
		/// 网站标题
		/// 格式见 WebsiteSettings.DocumentTitleFormat
		/// <example>
		/// {{ "Website Title" | website_title }}
		/// </example>
		/// </summary>
		/// <param name="text">需要翻译的文本</param>
		/// <returns></returns>
		public static string WebsiteTitle(string title) {
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var settings = configManager.GetData<WebsiteSettings>();
			if (!string.IsNullOrEmpty(settings.DocumentTitleFormat)) {
				title = (settings.DocumentTitleFormat
					.Replace("{title}", new T(title))
					.Replace("{websiteName}", new T(settings.WebsiteName)));
			}
			return title;
		}

		/// <summary>
		/// 全局过滤网址
		/// <example>
		/// {{ "/example" | url }}
		/// </example>
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static string Url(string url) {
			var filters = Application.Ioc.ResolveMany<IUrlFilter>();
			foreach (var filter in filters) {
				filter.Filter(ref url);
			}
			return url;
		}

		/// <summary>
		/// 获取Url参数，传入的url是空值时使用当前请求的url
		/// <example>
		/// {{ "" | url_get_param: "key" }}
		/// {{ test_url | url_get_param: variable }}
		/// </example>
		/// </summary>
		/// <param name="url">来源url，空值时使用当前请求的url</param>
		/// <param name="key">参数</param>
		/// <returns></returns>
		public static string UrlGetParam(string url, string key) {
			if (string.IsNullOrEmpty(url)) {
				url = HttpManager.CurrentContext.Request.Url.PathAndQuery;
			}
			var queryIndex = url.IndexOf('?');
			if (queryIndex < 0) {
				return null;
			}
			var query = HttpUtility.ParseQueryString(url.Substring(queryIndex + 1));
			return query[key];
		}

		/// <summary>
		/// 设置Url参数，传入的url是空值时使用当前请求的url
		/// <example>
		/// {{ "" | url_set_param: "key", "value" | url }}
		/// {{ test_url | url_set_param: "key", variable | url }}
		/// </example>
		/// </summary>
		/// <param name="url">来源url，空值时使用当前请求的url</param>
		/// <param name="key">参数</param>
		/// <param name="value">参数值，等于null时表示移除</param>
		/// <returns></returns>
		public static string UrlSetParam(string url, string key, object value = null) {
			if (string.IsNullOrEmpty(url)) {
				url = HttpManager.CurrentContext.Request.Url.PathAndQuery;
			}
			var queryIndex = url.IndexOf('?');
			var path = queryIndex >= 0 ? url.Substring(0, queryIndex) : url;
			var query = HttpUtility.ParseQueryString(
				queryIndex >= 0 ? url.Substring(queryIndex + 1) : "");
			if (value == null) {
				query.Remove(key);
			} else {
				query.Set(key, value.ToString());
			}
			var urlBuilder = new StringBuilder();
			urlBuilder.Append(path);
			if (query.Count > 0) {
				urlBuilder.Append('?').Append(query.ToString());
			}
			return urlBuilder.ToString();
		}

		/// <summary>
		/// 删除Url参数，传入的url是空值时使用当前请求的url
		/// 等于UrlSetParam(url, key, null)
		/// <example>
		/// {{ "" | url_remove_param: "key" }}
		/// {{ test_url | url_remove_param: variable }}
		/// </example>
		/// </summary>
		/// <param name="url">来源url，空值时使用当前请求的url</param>
		/// <param name="key">参数</param>
		/// <returns></returns>
		public static string UrlRemoveParam(string url, string key) {
			return UrlSetParam(url, key);
		}
	}
}
