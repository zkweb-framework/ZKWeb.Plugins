using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Config;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Base.src.TemplateFilters {
	/// <summary>
	/// 模板系统的过滤器
	/// </summary>
	public static class Filters {
		/// <summary>
		/// 网站标题
		/// <example>{{ "Website Title" | website_title }}</example>
		///	格式见 WebsiteSettings.DocumentTitleFormat
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
		/// <example>{{ "/example" | url }}</example>
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
		/// 替换Url参数，传入的url是空值时使用当前请求的url
		/// <example>
		/// {{ "" | url_replace_param: "key", "value" | url }}
		/// {{ test_url | url_replace_param: "key", variable | url }}
		/// </example>
		/// </summary>
		/// <param name="url">来源url，空值时使用当前请求的url</param>
		/// <param name="key">参数</param>
		/// <param name="value">参数值，等于null时表示移除</param>
		/// <returns></returns>
		public static string UrlReplaceParam(string url, string key, object value = null) {
			if (string.IsNullOrEmpty(url)) {
				url = HttpContextUtils.CurrentContext.Request.Url.PathAndQuery;
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
	}
}
