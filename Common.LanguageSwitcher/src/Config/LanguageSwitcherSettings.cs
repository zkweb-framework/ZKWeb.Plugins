using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.LanguageSwitcher.src.Config {
	/// <summary>
	/// 语言切换设置
	/// </summary>
	[GenericConfig("Common.LanguageSwitcher.LanguageSwitcherSettings", CacheTime = 15)]
	public class LanguageSwitcherSettings : ILiquidizable {
		/// <summary>
		/// 可以提供切换的语言列表
		/// </summary>
		public List<string> SwitchableLanguages { get; set; }
		/// <summary>
		/// 在前台页面显示语言切换器
		/// </summary>
		public bool DisplaySwitcherOnFrontPages { get; set; }
		/// <summary>
		/// 在后台页面显示语言切换器
		/// </summary>
		public bool DisplaySwitcherOnAdminPanel { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public LanguageSwitcherSettings() {
			SwitchableLanguages = new List<string>() {
				"zh-CN", "zh-TW", "ko-KR", "en-US", "ja-JP"
			};
			DisplaySwitcherOnFrontPages = true;
			DisplaySwitcherOnAdminPanel = true;
		}

		/// <summary>
		/// 允许描画到模板
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return new {
				SwitchableLanguages,
				DisplaySwitcherOnFrontPages,
				DisplaySwitcherOnAdminPanel
			};
		}
	}
}
