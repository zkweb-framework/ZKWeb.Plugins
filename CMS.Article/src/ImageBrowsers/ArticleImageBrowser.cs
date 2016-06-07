using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.CMS.ImageBrowser.src.Scaffolding;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.CMS.Article.src.ImageBrowsers {
	/// <summary>
	/// 文章图片浏览器
	/// </summary>
	[ExportMany]
	public class ArticleImageBrowser : ImageBrowserBuilder {
		public override string Category { get { return "Article"; } }
	}
}
