using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.CMS.ImageBrowser.src.Scaffolding;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.Article.src.ImageBrowsers {
	/// <summary>
	/// 文章图片浏览器
	/// </summary>
	[ExportMany]
	public class ArticleImageBrowser : ImageBrowserBuilder {
		public override string Category { get { return "Article"; } }
	}
}
