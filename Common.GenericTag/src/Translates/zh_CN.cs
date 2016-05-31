using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.GenericTag.src.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Generic Tag", "通用标签" },
			{ "Generic tag management", "通用标签的管理" },
			{ "TagManage", "标签管理" },
			{ "DefaultTag", "默认标签" },
			{ "Try to access tag that type not matched", "尝试操作类型不匹配的标签" },
			{ "DisplayOrder", "显示顺序" },
			{ "Order from small to large", "从小到大排列" },
			// TODO: 以下未翻译到其他语言
			{ "Tag", "标签" },
			{ "Tags", "标签" },
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
