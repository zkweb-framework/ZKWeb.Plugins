using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ZKWeb.Localize;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Translates {
	/// <summary>
	/// 英文翻译
	/// 用于把"CamelCaseWord"转换为"Camel Case Word"
	/// </summary>
	[ExportMany, SingletonReuse]
	public class en_US : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "en-US", "en" };
		private Regex CamelCaseRegex = new Regex("([A-Z]+[a-z]+)+", RegexOptions.Compiled);

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			if (CamelCaseRegex.IsMatch(text)) {
				var translated = new StringBuilder(text.Length * 2);
				for (int n = 0; n < text.Length; ++n) {
					var c = text[n];
					if (char.IsUpper(c) && n > 0 && char.IsLower(text[n - 1])) {
						translated.Append(' ');
					}
					translated.Append(c);
				}
				return translated.ToString();
			}
			return null;
		}
	}
}
