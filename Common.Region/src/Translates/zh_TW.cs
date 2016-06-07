using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.Region.src.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Region", "區域" },
			{ "Provide regions and related functions", "提供區域列表和相關的功能" },
			{ "RegionSettings", "地區設置" },
			{ "DefaultCountry", "默認國家/行政區" },
			{ "DisplayCountryDropdown", "顯示國家/行政區下拉框" },
			{ "CN", "中華人民共和國" },
			{ "US", "美利堅合眾國" },
			{ "FR", "法國" },
			{ "GB", "英國" },
			{ "DE", "德國" },
			{ "JP", "日本國" },
			{ "KR", "大韓民國" },
			{ "ES", "西班牙王國" },
			{ "TH", "泰國" },
			{ "TW", "台湾" },
			{ "HK", "香港特別行政區" },
			{ "RU", "俄羅斯聯邦" },
			{ "IT", "意大利共和國" },
			{ "GR", "希腊共和國" },
			{ "AE", "阿拉伯聯合酋長國" },
			{ "PL", "波蘭共和國" },
			{ "CZ", "捷克共和國" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
