using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Captcha.src.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Captcha", "キャプチャ" },
			{ "Click to change captcha image", "クリックでキャプチャ画像を変更します" },
			{ "Please enter captcha", "キャプチャを入力してください" },
			{ "Incorrect captcha", "キャプチャが違います、もう一度入力してください" },
			{ "Provide captcha form field and validation", "キャプチャフォームフィールドと認証機能を提供する" },
			{ "Captcha Audio", "キャプチャ音声" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
