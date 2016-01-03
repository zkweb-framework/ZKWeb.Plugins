using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Base.src.Config {
	/// <summary>
	/// 语言时区设置
	/// </summary>
	[GenericConfig("Common.Base", "LocaleSettings", CacheTime = 15)]
	public class LocaleSettings {
		/// <summary>
		/// 默认语言
		/// </summary>
		public string DefaultLanguage { get; set; }
		/// <summary>
		/// 默认时区
		/// </summary>
		public string DefaultTimezone { get; set; }
		/// <summary>
		/// 是否允许自动检测浏览器语言
		/// </summary>
		public bool AllowDetectLanguageFromBrowser { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public LocaleSettings() {
			DefaultLanguage = "zh-CN";
			DefaultTimezone = "China Standard Time";
			AllowDetectLanguageFromBrowser = false;
		}
	}
}
