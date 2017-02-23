using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "VisualThemeEditor", "可視化編輯器" },
			{ "VisualEditor", "可視化編輯" },
			{ "Allow edit website theme visually", "允許可視化編輯網站主題" },
			{ "AddElement", "添加元素" },
			{ "ManageTheme", "管理主題" },
			{ "SwitchPage", "切換頁面" },
			{ "SaveChanges", "保存修改" },
			{ "Please click the page link you want to switch to", "請點擊您想切換到的頁面的鏈接" },
			{ "Make sure you have saved all the changes, otherwise they will be lost.",
				"請確認您已保存所有更改, 否則這些更改將會丟失." },
			{ "EnterVisualEditor", "進入可視化編輯" },
			{ "NoDescription", "無描述" },
			{ "RemoveElement", "刪除元素" },
			{ "Are you sure to remove $element?", "確認刪除$element?" },
			{ "Add Element Success", "添加元素成功" },
			{ "Remove Element Success", "刪除元素成功" },
			{ "Edit Element Success", "編輯元素成功" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
