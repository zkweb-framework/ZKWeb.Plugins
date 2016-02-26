using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.LanguageSwitcher.src.Config {
	/// <summary>
	/// 语言切换设置
	/// </summary>
	[GenericConfig("Common.LanguageSwitcher.LanguageSwitcherSettings", CacheTime = 15)]
	public class LanguageSwitcherSettings {
		/// <summary>
		/// 可以提供切换的语言列表
		/// </summary>
		public List<string> SwitchableLanguages { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public LanguageSwitcherSettings() {
			SwitchableLanguages = new List<string>();
		}
	}
}
