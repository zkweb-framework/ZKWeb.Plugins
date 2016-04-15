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
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Captcha", "验证码" },
			{ "Click to change captcha image", "点击更换验证码图片" },
			{ "Please enter captcha", "请填写验证码" },
			{ "Incorrect captcha", "验证码错误，请重新填写" },
			{ "Provide captcha form field and validation", "提供验证码表单字段和验证功能" },
			{ "Captcha Audio", "验证码语音" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
