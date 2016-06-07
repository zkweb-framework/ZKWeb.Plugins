using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugin.Interfaces;
using ZKWeb.Templating.AreaSupport;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.CMS.Article.src {
	/// <summary>
	/// 载入插件时的处理
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Plugin : IPlugin {
		public Plugin() {
			// 注册默认模块
			var areaManager = Application.Ioc.Resolve<TemplateAreaManager>();
			// 文章详情页
			areaManager.GetArea("article_contents").DefaultWidgets.Add("cms.article.widgets/article_contents");
			// 文章列表页
			areaManager.GetArea("article_list_nav").DefaultWidgets.Add("cms.article.widgets/article_list_nav");
			areaManager.GetArea("article_list_table").DefaultWidgets.Add("cms.article.widgets/article_list_table");
			// 文章导航栏
			areaManager.GetArea("header_menubar").DefaultWidgets.Add("cms.article.widgets/article_nav_menu");
		}
	}
}
