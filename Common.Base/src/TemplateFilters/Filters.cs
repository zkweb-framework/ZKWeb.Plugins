using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Config;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;

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
	}
}
