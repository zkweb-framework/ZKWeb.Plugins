using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Region.src.Translates {
	/// <summary>
	/// 英文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class en_US : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "en-US", "en" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "CN", "China" },
			{ "US", "United States" },
			{ "FR", "France" },
			{ "GB", "United Kingdom" },
			{ "DE", "Germany" },
			{ "JP", "Japan" },
			{ "KR", "South Korea" },
			{ "ES", "Spain" },
			{ "TH", "Thai" },
			{ "TW", "Taiwan" },
			{ "HK", "Hong Kong SAR" },
			{ "RU", "Russia" },
			{ "IT", "Italy" },
			{ "GR", "Greece" },
			{ "AE", "United Arab Emirates" },
			{ "PL", "Poland" },
			{ "CZ", "Czech" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
