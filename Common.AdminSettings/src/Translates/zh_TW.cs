using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.AdminSettings.src.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "AdminSettings", "後台設置" },
			{ "Centralized management for admin settings", "提供對後台設置的集中管理" },
			{ "Settings", "設置" },
			{ "BaseSettings", "基本設置" },
			{ "WebsiteSettings", "網站設置" },
			{ "LocaleSettings", "語言設置" },
			{ "Please click the option you want to change on sidebar", "請在側邊欄中選擇您想要修改的設置" },
			{ "WebsiteName", "網站名稱" },
			{ "DocumentTitleFormat", "標題格式" },
			{ "Default is {title} - {websiteName}", "默認是{title} - {websiteName}"},
			{ "CopyrightText", "版權信息" },
			{ "DefaultLanguage", "默認語言" },
			{ "DefaultTimezone", "默認時區" },
			{ "AllowDetectLanguageFromBrowser", "使用瀏覽器指定的語言" },
			{ "ar-DZ", "阿拉伯語" },
			{ "zh-CN", "中文" },
			{ "cs-CZ", "捷克語" },
			{ "en-US", "英語" },
			{ "fr-FR", "法語" },
			{ "de-DE", "德語" },
			{ "el-GR", "希腊語" },
			{ "it-IT", "意大利語" },
			{ "ja-JP", "日語" },
			{ "ko-KR", "韓語" },
			{ "pl-PL", "波蘭語" },
			{ "ru-RU", "俄語" },
			{ "es-ES", "西班牙語" },
			{ "th-TH", "泰語" },
			{ "zh-TW", "繁體中文" },
			{ "FrontPageLogo", "前台Logo" },
			{ "AdminPanelLogo", "後台Logo" },
			{ "RestoreDefaultFrontPageLogo", "恢復默認前台Logo" },
			{ "RestoreDefaultAdminPanelLogo", "恢復默認後台Logo" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
