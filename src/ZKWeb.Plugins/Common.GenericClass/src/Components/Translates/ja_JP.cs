using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericClass.src.Components.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Generic Class", "汎用クラス" },
			{ "Generic class/catalog management", "汎用クラスの管理" },
			{ "ClassManage", "クラス管理" },
			{ "DefaultClass", "デフォルトクラス" },
			{ "Try to access class that type not matched", "クラスタイプが不正です" },
			{ "DisplayOrder", "表示順番" },
			{ "Order from small to large", "昇順" },
			{ "ParentClass", "親クラス" },
			{ "Add Top Level Class", "トップレベルのクラスを追加" },
			{ "Add Same Level Class", "同レベルのクラスを追加" },
			{ "Add Child Class", "子クラスを追加" },
			{ "Class", "クラス" },
			{ "Classes", "クラス" },
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
