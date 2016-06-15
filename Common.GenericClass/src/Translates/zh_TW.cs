using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericClass.src.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Generic Class", "通用分類" },
			{ "Generic class/catalog management", "通用分類的管理" },
			{ "ClassManage", "分類管理" },
			{ "DefaultClass", "默認分類" },
			{ "Try to access class that type not matched", "嘗試操作類型不匹配的分類" },
			{ "DisplayOrder", "顯示順序" },
			{ "Order from small to large", "從小到大排列" },
			{ "ParentClass", "上級分類" },
			{ "Add Top Level Class", "添加頂級分類" },
			{ "Add Same Level Class", "添加同級分類" },
			{ "Add Child Class", "添加子分類" },
			{ "Class", "分類" },
			{ "Classes", "分類" },
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
