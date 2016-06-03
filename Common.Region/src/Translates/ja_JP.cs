using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Region.src.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Region", "地域" },
			{ "Provide regions and related functions", "地域リストと関連機能" },
			{ "RegionSettings", "地域設定" },
			{ "DefaultCountry", "デフォルトの国/行政エリア" },
			{ "DisplayCountryDropdown", "国/行政エリアのドロップダウンリストを表示する" },
			{ "CN", "中華人民共和国" },
			{ "US", "アメリカ合衆国" },
			{ "FR", "フランス" },
			{ "GB", "イギリス" },
			{ "DE", "ドイツ" },
			{ "JP", "日本国" },
			{ "KR", "大韓民国" },
			{ "ES", "スペイン王国" },
			{ "TH", "タイ" },
			{ "TW", "台湾" },
			{ "HK", "香港特别行政区" },
			{ "RU", "ロシア連邦" },
			{ "IT", "イタリア共和国" },
			{ "GR", "ギリシャ共和国" },
			{ "AE", "アラブ首長国連邦" },
			{ "PL", "ポーランド共和国" },
			{ "CZ", "チェコ" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
