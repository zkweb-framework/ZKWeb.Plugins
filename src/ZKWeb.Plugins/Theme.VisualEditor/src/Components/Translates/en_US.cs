using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.Translates {
	/// <summary>
	/// 英文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class en_US : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "en-US", "en" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "__InlineCss", "Inline Css" },
			{ "__BeforeHtml", "Before Html" },
			{ "__AfterHtml", "After Html" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
