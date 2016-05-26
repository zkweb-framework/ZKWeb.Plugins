using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.AdminSettings.src.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "AdminSettings", "管理者設定" },
			{ "Centralized management for admin settings", "管理者設定の集中管理" },
			{ "Settings", "設定" },
			{ "BaseSettings", "基本設定" },
			{ "WebsiteSettings", "ウェブサイト設定" },
			{ "LocaleSettings", "言語設定" },
			{ "Please click the option you want to change on sidebar", "サイドバーにある変更したい設定をクリックしてください" },
			{ "WebsiteName", "ウェブサイト名称" },
			{ "DocumentTitleFormat", "タイトルフォーマット" },
			{ "Default is {title} - {websiteName}", "デフォルトは{title} - {websiteName}"},
			{ "CopyrightText", "著作権情報" },
			{ "DefaultLanguage", "デフォルト言語" },
			{ "DefaultTimezone", "デフォルトタイムゾーン" },
			{ "AllowDetectLanguageFromBrowser", "ブラウザから使用言語を検出する" },
			{ "ar-DZ", "アラビア語" },
			{ "zh-CN", "中国語" },
			{ "cs-CZ", "チェコ語" },
			{ "en-US", "英語" },
			{ "fr-FR", "フランス語" },
			{ "de-DE", "ドイツ語" },
			{ "el-GR", "ギリシャ語" },
			{ "it-IT", "イタリア語" },
			{ "ja-JP", "日本語" },
			{ "ko-KR", "韓国語" },
			{ "pl-PL", "ポーランド語" },
			{ "ru-RU", "ロシア語" },
			{ "es-ES", "スペイン語" },
			{ "th-TH", "タイ語" },
			{ "zh-TW", "中国語繁体字" },
			{ "FrontPageLogo", "フロントページのロゴ" },
			{ "AdminPanelLogo", "管理者パネルのロゴ" },
			{ "RestoreDefaultFrontPageLogo", "フロントページのロゴをデフォルトに戻す" },
			{ "RestoreDefaultAdminPanelLogo", "管理者パネルのロゴをデフォルトに戻す" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
