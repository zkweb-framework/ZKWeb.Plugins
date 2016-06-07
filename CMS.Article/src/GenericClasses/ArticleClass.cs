using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.GenericClass.src.Scaffolding;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.CMS.Article.src.GenericClasses {
	/// <summary>
	/// 文章分类
	/// </summary>
	[ExportMany]
	public class ArticleClass : GenericClassBuilder {
		public override string Name { get { return "ArticleClass"; } }
	}
}
