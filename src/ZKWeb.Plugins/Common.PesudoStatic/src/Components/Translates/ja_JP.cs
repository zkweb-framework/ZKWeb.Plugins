using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.Components.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>() {
			{ "PesudoStatic", "擬似静的ページ" },
			{ "Pesudo static support", "擬似静的ページのサポート" },
			{ "PesudoStaticSettings", "擬似静的ページの設定" },
			{ "EnablePesudoStatic", "擬似静的ページを有効にする" },
			{ "PesudoStaticExtension", "擬似静的ページの拡張子" },
			{ "PesudoStaticParamDelimiter", "擬似静的ページのパラメータの区切り文字" },
			{ "PesudoStaticPolicy", "擬似静的ページのポリシー" },
			{ "BlackListPolicy", "ブラックリストポリシー" },
			{ "WhiteListPolicy", "ホワイトリストポリシー" },
			{ "IncludeUrlPaths", "含めるURLパス" },
			{ "ExcludeUrlPaths", "含めないURLパス" },
			{ "One path per line, only available for whitelist policy",
				"一行に一つ、ホワイトリストポリシーでのみ有効" },
			{ "One path per line, only available for blacklist policy",
				"一行に一つ、ブラックリストポリシーでのみ有効" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
