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
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Article", "文章" },
			{ "ArticleManage", "文章管理" },
			{ "Article management", "文章管理" },
			{ "Title/Summary/Author", "標題/摘要/作者" },
			{ "ArticleClass", "文章分類" },
			{ "ArticleTag", "文章標簽" },
			{ "Title", "標題" },
			{ "Author", "作者" },
			{ "Summary", "摘要" },
			{ "ArticleList", "文章列表" },
			{ "AllArticles", "全部文章" },
			{ "Posted on", "發表在" },
			{ "Read More", "閱讀更多" },
			{ "No matching articles found, please change the condition and search again.",
				"沒有找到匹配的文章，請使用其他條件再次搜索。"},
			{ "The article you are visiting does not exist.", "您查看的文章不存在" },
			{ "Preview", "預覽" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
