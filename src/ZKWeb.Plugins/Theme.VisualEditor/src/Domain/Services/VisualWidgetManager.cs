using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Theme.VisualEditor.src.Components.ExtraConfigKeys;
using ZKWeb.Plugins.Theme.VisualEditor.src.Components.VisualWidgetsProviders.Interfaces;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs;
using ZKWeb.Server;
using ZKWeb.Templating;
using ZKWeb.Templating.DynamicContents;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Services {
	/// <summary>
	/// 可视化编辑模块管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class VisualWidgetManager : DomainServiceBase {
		/// <summary>
		/// 模块信息的缓存时间
		/// 默认是15秒，可通过网站配置指定
		/// </summary>
		public TimeSpan WidgetsCacheTime { get; protected set; }
		/// <summary>
		/// 模块信息的缓存
		/// </summary>
		public IKeyValueCache<int, IList<VisualWidgetGroup>> WidgetsCache { get; protected set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public VisualWidgetManager() {
			var configManager = Application.Ioc.Resolve<WebsiteConfigManager>();
			var cacheFactory = Application.Ioc.Resolve<ICacheFactory>();
			var extra = configManager.WebsiteConfig.Extra;
			WidgetsCacheTime = TimeSpan.FromSeconds(extra.GetOrDefault(
				VisualEditorExtraConfigKeys.WidgetsCacheTime, 15));
			WidgetsCache = cacheFactory.CreateCache<int, IList<VisualWidgetGroup>>();
		}

		/// <summary>
		/// 获取模块信息列表
		/// 以分组形式返回
		/// </summary>
		/// <returns></returns>
		public virtual IList<VisualWidgetGroup> GetWidgets() {
			return WidgetsCache.GetOrCreate(0, () => {
				var providers = Application.Ioc.ResolveMany<IVisualWidgetsProvider>();
				var widgets = providers
					.SelectMany(p => p.GetWidgets())
					.GroupBy(p => p.Group)
					.Select(p => new VisualWidgetGroup(
						p.Key, p.OrderBy(w => w.WidgetInfo.WidgetPath).ToList()))
					.OrderBy(p => p.Group)
					.ToList();
				return widgets;
			}, WidgetsCacheTime);
		}

		/// <summary>
		/// 获取模块的Html
		/// </summary>
		/// <param name="url">模块所在的Url地址</param>
		/// <param name="path">模块的路径</param>
		/// <param name="args">模块的参数</param>
		/// <returns></returns>
		public virtual string GetWidgetHtml(string url, string path, IDictionary<string, object> args) {
			// 获取模块的Html之前首先要获取到所在Url的TemplateResult, 模块有可能需要用到返回的变量
			var uri = new Uri(url);
			var pageManager = Application.Ioc.Resolve<VisualPageManager>();
			var templateResult = pageManager.GetPageResult(uri.PathAndQuery) as TemplateResult;
			// 获取模块的Html
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var areaManager = Application.Ioc.Resolve<TemplateAreaManager>();
			var context = new DotLiquid.Context();
			var widget = new TemplateWidget(path, args);
			if (templateResult?.TemplateArgument != null) {
				var arguments = templateResult.TemplateArgument;
				context.Push(templateManager.CreateHash(arguments));
			}
			var widgetHtml = areaManager.RenderWidget(context, widget);
			return widgetHtml;
		}
	}
}
