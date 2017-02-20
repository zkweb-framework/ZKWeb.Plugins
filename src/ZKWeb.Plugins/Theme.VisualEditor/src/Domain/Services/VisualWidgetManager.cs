using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Theme.VisualEditor.src.Components.ExtraConfigKeys;
using ZKWeb.Plugins.Theme.VisualEditor.src.Components.VisualWidgetsProviders.Interfaces;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs;
using ZKWeb.Server;
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
	}
}
