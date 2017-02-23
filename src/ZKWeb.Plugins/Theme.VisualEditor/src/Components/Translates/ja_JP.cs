using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "VisualThemeEditor", "ビジュアルエディタ" },
			{ "VisualEditor", "ビジュアルエディタ" },
			{ "Allow edit website theme visually", "視覚的にウェブサイトのテーマを編集できる" },
			{ "AddElement", "要素追加" },
			{ "ManageTheme", "テーマ管理" },
			{ "SwitchPage", "ページ切り替え" },
			{ "SaveChanges", "変更保存" },
			{ "Please click the page link you want to switch to", "切り替えたいページのリンクをクリックしてください" },
			{ "Make sure you have saved all the changes, otherwise they will be lost.",
				"すべての変更を保存したことを確認してください, そうしないと変更が失われます." },
			{ "EnterVisualEditor", "ビジュアルエディタに入る" },
			{ "NoDescription", "説明なし" },
			{ "RemoveElement", "要素削除" },
			{ "Are you sure to remove $element?", "$elementを削除してもよろしいですか?" },
			{ "Add Element Success", "要素の追加に成功しました" },
			{ "Remove Element Success", "要素の削除に成功しました" },
			{ "Edit Element Success", "要素の編集に成功しました" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
