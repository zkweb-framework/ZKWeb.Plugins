using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Captcha.src.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Captcha", "驗證碼" },
			{ "Click to change captcha image", "點擊更換驗證碼圖片" },
			{ "Please enter captcha", "請填寫驗證碼" },
			{ "Incorrect captcha", "驗證碼錯誤，請重新填寫" },
			{ "Provide captcha form field and validation", "提供驗證碼表單字段和驗證功能" },
			{ "Captcha Audio", "驗證碼語音" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
