using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "CustomTranslate", "カスタム翻訳" },
			{ "Support custom translate through admin panel", "カスタム翻訳の管理" },
			{ "Translation", "翻訳内容" },
			{ "Origin/Translated", "オリジナル/翻訳" },
			{ "OriginalText", "オリジナル" },
			{ "TranslatedText", "翻訳" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
