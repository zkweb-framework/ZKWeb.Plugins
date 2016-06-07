using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.GenericTag.src.Scaffolding;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.CMS.Article.src.GenericTags {
	/// <summary>
	/// 文章标签
	/// </summary>
	[ExportMany]
	public class ArticleTag : GenericTagBuilder {
		public override string Name { get { return "ArticleTag"; } }
	}
}
