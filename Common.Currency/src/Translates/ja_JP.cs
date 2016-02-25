using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Currency.src.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Currency", "貨幣" },
			{ "Provide currency types and related functions", "貨幣の種類と関連機能" },
			{ "CurrencySettings", "貨幣設定" },
			{ "DefaultCurrency", "デフォルト貨幣" },
			{ "CNY", "人民元" },
			{ "USD", "ドル" },
			{ "EUR", "ユーロ" },
			{ "JPY", "日本円" },
			{ "HKD", "香港ドル" },
			{ "TWD", "台湾ドル" },
			{ "KRW", "韓国ウォン" },
			{ "RUB", "ルーブル" },
			{ "THB", "タイバーツ" },
			{ "CHF", "スイスフラン" },
			{ "CZK", "チェココルナ" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
