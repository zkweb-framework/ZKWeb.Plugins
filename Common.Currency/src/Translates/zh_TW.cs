using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.Currency.src.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Currency", "貨幣" },
			{ "Provide currency types and related functions", "提供貨幣類型和相關的功能" },
			{ "CurrencySettings", "貨幣設置" },
			{ "DefaultCurrency", "默認貨幣" },
			{ "CNY", "人民幣" },
			{ "USD", "美元" },
			{ "EUR", "歐元" },
			{ "JPY", "日元" },
			{ "HKD", "港元" },
			{ "TWD", "新台幣" },
			{ "KRW", "韓元" },
			{ "RUB", "盧布" },
			{ "THB", "泰銖" },
			{ "CHF", "瑞士法郎" },
			{ "CZK", "捷克克朗" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
