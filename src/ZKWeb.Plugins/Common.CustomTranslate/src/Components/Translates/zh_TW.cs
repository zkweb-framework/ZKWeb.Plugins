using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Components.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "CustomTranslate", "定制翻譯" },
			{ "Support custom translate through admin panel", "支持在管理後台中設置定制翻譯" },
			{ "Translation", "翻譯內容" },
			{ "Origin/Translated", "原文/譯文" },
			{ "OriginalText", "原文" },
			{ "TranslatedText", "譯文" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
