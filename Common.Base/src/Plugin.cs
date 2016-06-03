using DotLiquid;
using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.TemplateFilters;
using ZKWeb.Plugins.Common.Base.src.TemplateTags;
using ZKWeb.Templating.AreaSupport;
using ZKWeb.Plugin.Interfaces;

namespace ZKWeb.Plugins.Common.Base.src {
	/// <summary>
	/// 载入插件时的处理
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Plugin : IPlugin {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin() {
			// 初始化定时任务管理器
			Application.Ioc.Resolve<ScheduledTaskManager>();
			// 注册模板标签和过滤器
			Template.RegisterTag<IncludeCssHere>("include_css_here");
			Template.RegisterTag<IncludeCssLater>("include_css_later");
			Template.RegisterTag<IncludeJsHere>("include_js_here");
			Template.RegisterTag<IncludeJsLater>("include_js_later");
			Template.RegisterTag<RenderIncludedCss>("render_included_css");
			Template.RegisterTag<RenderIncludedJs>("render_included_js");
			Template.RegisterTag<RenderMetadata>("render_metadata");
			Template.RegisterTag<RenderTitle>("render_title");
			Template.RegisterTag<UrlPagination>("url_pagination");
			Template.RegisterTag<UseSeoDescription>("use_seo_description");
			Template.RegisterTag<UseSeoKeywords>("use_seo_keywords");
			Template.RegisterTag<UseTitle>("use_title");
			Template.RegisterTag<WebsiteName>("website_name");
			Template.RegisterFilter(typeof(Filters));
			// 注册模板可描画类型
			Template.RegisterSafeType(typeof(MenuItem), s => s);
			// 注册默认模块
			var areaManager = Application.Ioc.Resolve<TemplateAreaManager>();
			areaManager.GetArea("header_logobar").DefaultWidgets.Add("common.base.widgets/logo");
			areaManager.GetArea("index_top_area_1").DefaultWidgets.Add("common.base.widgets/index_help");
			areaManager.GetArea("footer_area_3").DefaultWidgets.Add("common.base.widgets/copyright");
		}
	}
}
