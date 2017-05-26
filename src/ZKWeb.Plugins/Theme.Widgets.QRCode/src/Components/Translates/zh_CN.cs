using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Theme.Widgets.QRCode.src.Components.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			// TODO: 翻译到其他语言
			{ "QRCodeTemplateWidgets", "二维码模板模块" },
			{ "QRCode template widgets", "二维码的模板模块" },
			{ "ECCLevel", "ECC等级" },
			{ "L, M, Q, H, Default is Q", "L, M, Q, H, 默认是Q" },
			{ "QRCodeDensity", "二维码密度" },
			{ "QRCode Density, Default is 20", "二维码密度, 默认是20" },
			{ "QRCodeSize", "二维码大小" },
			{ "Default is 100", "默认是100" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
