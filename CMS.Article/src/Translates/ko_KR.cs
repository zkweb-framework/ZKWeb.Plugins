using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.CMS.Article.src.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Article", "기사" },
			{ "ArticleManage", "문서 관리" },
			{ "Article management", "문서 관리" },
			{ "Title/Summary/Author", "표제/개요/저자" },
			{ "ArticleClass", "제 카테고리" },
			{ "ArticleTag", "기술 자료 태그" },
			{ "Title", "표제" },
			{ "Author", "저자" },
			{ "Summary", "개요" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
