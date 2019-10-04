using DotLiquid;
using System;
using ZKWeb.Templating.DynamicContents;
using ZKWebStandard.Ioc;
using ZKWeb.Plugin;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.TemplateTags;
using ZKWeb.Plugins.Common.Base.src.UIComponents.TemplateFilters;
using ZKWeb.Plugins.Common.Base.src.UIComponents.MenuItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.HtmlItems;

namespace ZKWeb.Plugins.Common.Base.src {
	/// <summary>
	/// 载入插件时的处理
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Plugin : IPlugin, IDisposable {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin() {
			// 初始化定时任务管理器
			Application.Ioc.Resolve<ScheduledTaskManager>();
			// 注册模板标签和过滤器
			Template.RegisterTag<CopyrightText>("copyright_text");
			Template.RegisterTag<IncludeCssHere>("include_css_here");
			Template.RegisterTag<IncludeCssLater>("include_css_later");
			Template.RegisterTag<IncludeJsHere>("include_js_here");
			Template.RegisterTag<IncludeJsLater>("include_js_later");
			Template.RegisterTag<RenderExtraMetadata>("render_extra_metadata");
			Template.RegisterTag<RenderIncludedCss>("render_included_css");
			Template.RegisterTag<RenderIncludedJs>("render_included_js");
			Template.RegisterTag<RenderMetaKeywords>("render_meta_keywords");
			Template.RegisterTag<RenderMetaDescription>("render_meta_description");
			Template.RegisterTag<RenderTitle>("render_title");
			Template.RegisterTag<UrlPagination>("url_pagination");
			Template.RegisterTag<UseMetaDescription>("use_meta_description");
			Template.RegisterTag<UseMetaKeywords>("use_meta_keywords");
			Template.RegisterTag<UseTitle>("use_title");
			Template.RegisterTag<WebsiteName>("website_name");
			Template.RegisterFilter(typeof(BaseFilters));
			// 注册模板可描画类型
			Template.RegisterSafeType(typeof(HtmlItem), s => s);
			Template.RegisterSafeType(typeof(MenuItem), s => s);
			// 注册默认模块
			var areaManager = Application.Ioc.Resolve<TemplateAreaManager>();
			areaManager.GetArea("header_logobar").DefaultWidgets.Add("common.base.widgets/logo");
			areaManager.GetArea("index_top_area_1").DefaultWidgets.Add("common.base.widgets/index_help");
			areaManager.GetArea("footer_area_3").DefaultWidgets.Add("common.base.widgets/copyright");
		}

		/// <summary>
		/// 卸载
		/// </summary>
		public void Dispose() {
			Application.Ioc.Resolve<ScheduledTaskManager>().Dispose();
		}
	}
}
