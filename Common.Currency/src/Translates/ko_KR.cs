using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Currency.src.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Currency", "통화" },
			{ "Provide currency types and related functions", "통화 유형을 제공 하 고 관련 기능" },
			{ "CurrencySettings", "통화 설정" },
			{ "DefaultCurrency", "기본 통화" },
			{ "CNY", "사람들" },
			{ "USD", "달러 미국" },
			{ "EUR", "유로" },
			{ "JPY", "일본 엔" },
			{ "HKD", "HKD" },
			{ "TWD", "TWD" },
			{ "KRW", "한국 원" },
			{ "RUB", "루블" },
			{ "THB", "태국 바트" },
			{ "CHF", "스위스 프랑" },
			{ "CZK", "체코 코루나" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
