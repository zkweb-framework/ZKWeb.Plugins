using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.CodeEditor.src.UIComponents.FormFieldAttributes;
using ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm;
using ZKWeb.Plugins.Theme.VisualEditor.src.Components.ExtraConfigKeys;
using ZKWeb.Plugins.Theme.VisualEditor.src.Components.TemplateWidgetRenderers;
using ZKWeb.Plugins.Theme.VisualEditor.src.Components.VisualWidgetsProviders.Interfaces;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs;
using ZKWeb.Server;
using ZKWeb.Templating;
using ZKWeb.Templating.DynamicContents;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

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
			// 过滤空参数, 无参数时应该等于null
			args = args.Where(x => x.Value != null && x.Value as string != "")
				.ToDictionary(x => x.Key, x => x.Value);
			if (args.Count == 0) {
				args = null;
			}
			// 获取模块的Html
			// 通过渲染器获取避免读取和写入区域管理器的缓存
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var renderer = Application.Ioc.Resolve<ITemplateWidgetRenderer>();
			var context = new DotLiquid.Context();
			var widget = new TemplateWidget(path, args);
			if (templateResult?.TemplateArgument != null) {
				var arguments = templateResult.TemplateArgument;
				context.Push(templateManager.CreateHash(arguments));
			}
			var widgetHtml = renderer.Render(context, widget);
			return widgetHtml;
		}

		/// <summary>
		/// 获取模块的编辑表单
		/// </summary>
		/// <param name="widgetInfo">模块信息</param>
		/// <returns></returns>
		public virtual FormBuilder GetWidgetEditForm(TemplateWidgetInfo widgetInfo) {
			var dynamicFormBuilder = new DynamicFormBuilder();
			// 添加模块中的参数
			dynamicFormBuilder.AddFields(widgetInfo.Arguments);
			// 生成表单
			var form = dynamicFormBuilder.ToForm<TabFormBuilder>();
			form.Attribute = new FormAttribute(
				"WidgetEditForm",
				"/api/visual_editor/submit_widget_edit_form?path=" +
				HttpUtils.UrlEncode(widgetInfo.WidgetPath));
			// 添加无参数的提醒
			if (!widgetInfo.Arguments.Any()) {
				form.Fields.Add(new FormField(new TemplateHtmlFieldAttribute(
					"NoArguments", "theme.visualeditor/no_arguments_alert.html")));
			}
			// 添加内嵌css, 前置html, 后置html
			var noLint = JsonConvert.SerializeObject(new { lint = false });
			form.Fields.Add(new FormField(new TextBoxFieldAttribute(
				VisualWidgetRenderer.CssClassKey, "example: col-md-3 my-class") { Group = "AdditionalStyle" }));
			form.Fields.Add(new FormField(new CodeEditorAttribute(
				VisualWidgetRenderer.InlineCssKey, 5, "css", noLint) { Group = "AdditionalStyle" }));
			form.Fields.Add(new FormField(new CodeEditorAttribute(
				VisualWidgetRenderer.BeforeHtmlKey, 8, "html", noLint) { Group = "AdditionalStyle" }));
			form.Fields.Add(new FormField(new CodeEditorAttribute(
				VisualWidgetRenderer.AfterHtmlKey, 8, "html", noLint) { Group = "AdditionalStyle" }));
			return form;
		}
	}
}
