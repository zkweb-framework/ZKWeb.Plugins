using System;
using System.Threading;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Base.src.Config {
	/// <summary>
	/// 语言时区设置
	/// </summary>
	[GenericConfig("Common.Base.LocaleSettings", CacheTime = 15)]
	public class LocaleSettings {
		/// <summary>
		/// 默认语言
		/// </summary>
		public string DefaultLanguage {
			get { return _DefaultLanguage ?? Thread.CurrentThread.CurrentCulture.Name; }
			set { _DefaultLanguage = value; }
		}
		private string _DefaultLanguage;
		/// <summary>
		/// 默认时区
		/// </summary>
		public string DefaultTimezone {
			get { return _DefaultTimezone ?? TimeZoneInfo.Local.StandardName; }
			set { _DefaultTimezone = value; }
		}
		private string _DefaultTimezone;
		/// <summary>
		/// 是否允许自动检测浏览器语言
		/// 默认不允许
		/// </summary>
		public bool AllowDetectLanguageFromBrowser { get; set; }
	}
}
