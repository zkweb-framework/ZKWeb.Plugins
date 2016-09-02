using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.Article.src.Components.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Article", "文章" },
			{ "ArticleManage", "文章管理" },
			{ "Article management and display", "提供文章管理和展示功能" },
			{ "Title/Summary/Author", "标题/摘要/作者" },
			{ "ArticleClass", "文章分类" },
			{ "ArticleTag", "文章标签" },
			{ "Title", "标题" },
			{ "Author", "作者" },
			{ "Summary", "摘要" },
			{ "ArticleList", "文章列表" },
			{ "AllArticles", "全部文章" },
			{ "Posted on", "发表在" },
			{ "Read More", "阅读更多" },
			{ "No matching articles found, please change the condition and search again.",
				"没有找到匹配的文章，请使用其他条件再次搜索。"},
			{ "The article you are visiting does not exist.", "您查看的文章不存在" },
			{ "Preview", "预览" },
			{ "ArticleSettings", "文章设置" },
			{ "ArticleListSettings", "文章列表设置" },
			{ "ArticlesPerPage", "每页显示的文章数量" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
