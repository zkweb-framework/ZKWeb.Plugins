using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Config;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.Currency.src.Config {
	/// <summary>
	/// 货币设置
	/// </summary>
	[GenericConfig("Common.Currency.CurrencySettings", CacheTime = 15)]
	public class CurrencySettings {
		/// <summary>
		/// 默认货币
		/// </summary>
		public string DefaultCurrency {
			get { return _DefaultCurrency ?? GetDefaultCurrencyByDefaultLanguage(); }
			set { _DefaultCurrency = value; }
		}
		private string _DefaultCurrency;

		/// <summary>
		/// 默认语言转换到默认货币的索引
		/// </summary>
		private static Dictionary<string, string> DefaultLanguageToCurrency = new Dictionary<string, string>() {
			{ "zh-CN", "CNY" },
			{ "en-US", "USD" },
			{ "fr-FR", "EUR" },
			{ "de-DE", "EUR" },
			{ "ja-JP", "JPY" },
			{ "ko-KR", "KRW" },
			{ "es-ES", "EUR" },
			{ "th-TH", "THB" },
			{ "zh-TW", "TWD" },
			{ "ru-RU", "RUB" },
			{ "it-IT", "EUR" },
			{ "el-GR", "EUR" },
			{ "ar-DZ", "USD" }, /* 阿拉伯没有统一的货币，这里默认美元 */
			{ "pl-PL", "EUR" },
			{ "cs-CZ", "CZK" }
		};

		/// <summary>
		/// 根据默认语言获取默认货币，找不到时返回USD
		/// </summary>
		/// <returns></returns>
		private static string GetDefaultCurrencyByDefaultLanguage() {
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var localeSettings = configManager.GetData<LocaleSettings>();
			return DefaultLanguageToCurrency.GetOrDefault(localeSettings.DefaultLanguage) ?? "USD";
		}
	}
}
