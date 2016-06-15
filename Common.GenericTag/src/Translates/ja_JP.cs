using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericTag.src.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Generic Tag", "汎用タグ" },
			{ "Generic tag management", "汎用タグの管理" },
			{ "TagManage", "タグ管理" },
			{ "DefaultTag", "デフォルトタグ" },
			{ "Try to access tag that type not matched", "タグタイプが不正です" },
			{ "DisplayOrder", "表示順番" },
			{ "Order from small to large", "昇順" },
			{ "Tag", "タグ" },
			{ "Tags", "タグ" },
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
