using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "ZKWeb Default Website", "ZKWebデフォルトサイト" },
			{ "Captcha", "キャプチャ" },
			{ "Click to change captcha image", "クリックでキャプチャ画像を変更します" },
			{ "Please enter captcha", "キャプチャを入力してください" },
			{ "Incorrect captcha", "キャプチャが違います、もう一度入力してください" },
			{ "{0} is required", "{0}を入力してください" },
			{ "Length of {0} must be {1}", "{0}の長さが{1}でなければなりません" },
			{ "Length of {0} must between {1} and {2}", "{0}の長さが{1}から{2}まででなければなりません" },
			{ "HomePage", "ホームページ" },
			{ "Index", "ホームページ" },
			{ "How to edit this page", "このページを編集するには" },
			{ "Use Plugin", "プラグインを使う" },
			{ "Copy Common.Base/templates/common.base/index.html to Your.Plugin/templates/common.base/index.html then edit it.",
				"Common.Base/templates/common.base/index.htmlからあなたのプライグイン/templates/common.base/index.htmlへコピーして編集を行う" },
			{ "Use Diy", "Diyを使う" },
			{ "Diy is not ready yet.", "Diy機能は未完成" },
			{ "Refresh", "リフレッシュ" },
			{ "Fullscreen", "フールスクリーン" },
			{ "Operations", "オペレーション" },
			{ "Export to excel", "Excelへエクスポート" },
			{ "Print", "プリント" },
			{ "Pagination Settings", "ページング設定" },
			{ "[0] Records per page", "ページごとに[0]項目" },
			{ "Please enter keyword", "キーワードを入力してください" },
			{ "Advance search", "高度な検索" },
			{ "Data with id {0} cannot be found", "Idが{0}のデータを見つかりませんでした" },
			{ "True", "真" },
			{ "False", "偽" },
			{ "Yes", "はい" },
			{ "No", "いいえ" },
			{ "Ok", "確認" },
			{ "Cancel", "キャンセル" },
			{ "Actions", "アクション" },
			{ "Deleted", "削除済み" },
			{ "Select All", "すべて選択" },
			{ "Select/Unselect All", "すべて選択/選択解除" },
			{ "Submit", "送信" },
			{ "Please Select", "選んでください" },
			{ "Only {0} files are allowed", "アップロードできるファイルは{0}のみです" },
			{ "Please upload file size not greater than {0}", "サイズが{0}を超えたファイルはアップロードできません" },
			{ "Basic Information", "基本情報" },
			{ "Base Functions", "基本機能" },
			{ "Base functions and template pages", "基本機能とテンプレートページ" },
			{ "{0} format is incorrect", "{0}のフォーマットが正しくありません" },
			{ "Expand/Collapse All", "すべて展開/折り畳み" },
			{ "Type", "タイプ" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
