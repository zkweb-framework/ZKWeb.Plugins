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
using ZKWeb.Templating.Diy;
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
			Template.RegisterTag<IncludeCss>("include_css");
			Template.RegisterTag<IncludeJs>("include_js");
			Template.RegisterTag<UseTitle>("use_title");
			Template.RegisterTag<WebsiteName>("website_name");
			Template.RegisterFilter(typeof(Filters));
			// 注册模板可描画类型
			Template.RegisterSafeType(typeof(MenuItem), s => s);
			// 注册默认模块
			var diyManager = Application.Ioc.Resolve<DiyManager>();
			diyManager.GetArea("header_logobar").DefaultWidgets.Add("common.base.widgets/logo");
		}
	}
}
