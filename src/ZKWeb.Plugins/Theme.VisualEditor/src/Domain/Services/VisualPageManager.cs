using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.Plugins.Common.Base.src.Controllers.Extensions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Theme.VisualEditor.src.Components.ExtraConfigKeys;
using ZKWeb.Plugins.Theme.VisualEditor.src.Components.VisualEditablePagesProviders.Interfaces;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs;
using ZKWeb.Server;
using ZKWeb.Web;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Services {
	/// <summary>
	/// 可视化编辑页面管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class VisualPageManager : DomainServiceBase {
		/// <summary>
		/// 可编辑的页面缓存时间
		/// 默认是15秒，可通过网站配置指定
		/// </summary>
		public TimeSpan EditablePagesCacheTime { get; protected set; }
		/// <summary>
		/// 可编辑的页面缓存
		/// </summary>
		public IKeyValueCache<int, IList<VisualEditablePageGroup>>
			EditablePagesCache { get; protected set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public VisualPageManager() {
			var configManager = Application.Ioc.Resolve<WebsiteConfigManager>();
			var cacheFactory = Application.Ioc.Resolve<ICacheFactory>();
			var extra = configManager.WebsiteConfig.Extra;
			EditablePagesCacheTime = TimeSpan.FromSeconds(extra.GetOrDefault(
				VisualEditorExtraConfigKeys.EditablePagesCacheTime, 15));
			EditablePagesCache = cacheFactory.CreateCache<int, IList<VisualEditablePageGroup>>();
		}

		/// <summary>
		/// 获取可编辑的页面列表
		/// 以分组形式返回
		/// </summary>
		/// <returns></returns>
		public virtual IList<VisualEditablePageGroup> GetEditablePages() {
			return EditablePagesCache.GetOrCreate(0, () => {
				var providers = Application.Ioc.ResolveMany<IVisualEditablePagesProvider>();
				var pages = providers
					.SelectMany(p => p.GetEditablePages())
					.GroupBy(p => p.Group)
					.Select(p => new VisualEditablePageGroup(p.Key, p.ToList()))
					.ToList();
				return pages;
			}, EditablePagesCacheTime);
		}

		/// <summary>
		/// 获取页面返回的模板结果对象
		/// 如果页面不存在则返回null
		/// </summary>
		/// <param name="pathAndQuery">页面的Url</param>
		/// <returns></returns>
		public virtual IActionResult GetPageResult(string pathAndQuery) {
			IActionResult result = null;
			using (HttpManager.OverrideContext(pathAndQuery, HttpMethods.GET)) {
				var controllerManager = Application.Ioc.Resolve<ControllerManager>();
				var wrappers = Application.Ioc.ResolveMany<IHttpRequestHandlerWrapper>();
				var getTemplateResult = new Action(() => {
					HttpManager.CurrentContext.SetIsEditingPage(true);
					var requestPath = HttpManager.CurrentContext.Request.Path;
					var action = controllerManager.GetAction(requestPath, HttpMethods.GET);
					result = action?.Invoke();
				});
				foreach (var wrapper in wrappers) {
					getTemplateResult = wrapper.WrapHandlerAction(getTemplateResult);
				}
				getTemplateResult();
			}
			return result;
		}
	}
}
