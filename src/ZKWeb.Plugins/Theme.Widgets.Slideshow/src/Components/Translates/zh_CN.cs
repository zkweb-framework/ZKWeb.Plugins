using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Theme.Widgets.Slideshow.src.Components.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			// TODO: 翻译到其他语言
			{ "SlideshowTemplateWidgets", "轮播模板模块" },
			{ "Slideshow template widgets", "轮播的模板模块" },
			{ "SlideShow", "轮播图" },
			{ "Image_1", "图片1" },
			{ "Image_2", "图片2" },
			{ "Image_3", "图片3" },
			{ "Image_4", "图片4" },
			{ "Image_5", "图片5" },
			{ "Image_6", "图片6" },
			{ "Image_7", "图片7" },
			{ "Image_8", "图片8" },
			{ "Image_9", "图片9" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
