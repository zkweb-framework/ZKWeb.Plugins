using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Config;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.Region.src.Config {
	/// <summary>
	/// 地区设置
	/// </summary>
	[GenericConfig("Common.Region.RegionSettings", CacheTime = 15)]
	public class RegionSettings {
		/// <summary>
		/// 默认国家/行政区
		/// </summary>
		public string DefaultCountry {
			get { return _DefaultCountry ?? GetDefaultCountryByDefaultLanguage(); }
			set { _DefaultCountry = value; }
		}
		private string _DefaultCountry;
		/// <summary>
		/// 是否显示国家下拉框
		/// </summary>
		public bool DisplayCountryDropdown { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public RegionSettings() {
			DisplayCountryDropdown = true;
		}

		/// <summary>
		/// 默认语言转换到默认国家/行政区的索引
		/// </summary>
		private static Dictionary<string, string> DefaultLanguageToCountry = new Dictionary<string, string>() {
			{ "zh-CN", "CN" },
			{ "en-US", "US" },
			{ "fr-FR", "FR" },
			{ "de-DE", "DE" },
			{ "ja-JP", "JP" },
			{ "ko-KR", "KR" },
			{ "es-ES", "ES" },
			{ "th-TH", "TH" },
			{ "zh-TW", "TW" },
			{ "ru-RU", "RU" },
			{ "it-IT", "IT" },
			{ "el-GR", "GR" },
			{ "ar-DZ", "AE" }, // 阿拉伯联合酋长国
			{ "pl-PL", "PL" },
			{ "cs-CZ", "CZ" }
		};

		/// <summary>
		/// 根据默认语言获取默认国家/行政区，找不到时返回US
		/// </summary>
		/// <returns></returns>
		private static string GetDefaultCountryByDefaultLanguage() {
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var localeSettings = configManager.GetData<LocaleSettings>();
			return DefaultLanguageToCountry.GetOrDefault(localeSettings.DefaultLanguage) ?? "US";
		}
	}
}
