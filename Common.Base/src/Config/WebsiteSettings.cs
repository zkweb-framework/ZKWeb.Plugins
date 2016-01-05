using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Base.src.Config {
	/// <summary>
	/// 网站设置
	/// </summary>
	[GenericConfig("Common.Minimal", "WebsiteSettings", CacheTime = 15)]
	public class WebsiteSettings {
		/// <summary>
		/// 网站名称
		/// </summary>
		public string WebsiteName { get; set; }
		/// <summary>
		/// 网页标题的默认格式，默认是 {title} - {websiteName}
		/// </summary>
		public string DocumentTitleFormat { get; set; }
		/// <summary>
		/// 版权信息
		/// </summary>
		public string CopyrightText { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public WebsiteSettings() {
			WebsiteName = "ZKWeb Default Website";
			DocumentTitleFormat = "{title} - {websiteName}";
			CopyrightText = "© 2016 ZKWeb";
		}
	}
}
