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
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Currency", "货币" },
			{ "Provide currency types and related functions", "提供货币类型和相关的功能" },
			{ "CurrencySettings", "货币设置" },
			{ "DefaultCurrency", "默认货币" },
			{ "CNY", "人民币" },
			{ "USD", "美元" },
			{ "EUR", "欧元" },
			{ "JPY", "日元" },
			{ "HKD", "港元" },
			{ "TWD", "新台币" },
			{ "KRW", "韩元" },
			{ "RUB", "卢布" },
			{ "THB", "泰铢" },
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
