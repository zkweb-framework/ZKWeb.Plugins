using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Admin.src.Components.ActionFilters;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.Controllers.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Services;
using ZKWeb.Templating.DynamicContents;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Controllers {
	/// <summary>
	/// 可视化编辑使用的Api控制器
	/// </summary>
	[ExportMany]
	public class VisualEditorApiController : ControllerBase {
		/// <summary>
		/// 获取顶部栏的Html
		/// </summary>
		/// <returns></returns>
		[Action("api/visual_editor/get_top_bar")]
		[CheckPrivilege(typeof(ICanUseAdminPanel), "VisualEditor:VisualEditor")]
		public IActionResult GetTopBar() {
			var widgetManager = Application.Ioc.Resolve<VisualWidgetManager>();
			var widgetGroups = widgetManager.GetWidgets();
			return new TemplateResult("theme.visualeditor/top_bar.html", new { widgetGroups });
		}

		/// <summary>
		/// 获取切换页面弹出框的Html
		/// </summary>
		/// <returns></returns>
		[Action("api/visual_editor/get_switch_pages")]
		[CheckPrivilege(typeof(ICanUseAdminPanel), "VisualEditor:VisualEditor")]
		public IActionResult GetSwitchPages() {
			var pageManager = Application.Ioc.Resolve<VisualPageManager>();
			var pageGroups = pageManager.GetEditablePages();
			return new TemplateResult("theme.visualeditor/switch_pages.html", new { pageGroups });
		}

		/// <summary>
		/// 获取编辑模块的表单
		/// </summary>
		[Action("api/visual_editor/get_widget_edit_form", HttpMethods.POST)]
		public IActionResult GetWidgetEditForm(string path, IDictionary<string, object> args) {
			// 获取模块信息
			var areaManager = Application.Ioc.Resolve<TemplateAreaManager>();
			var widgetInfo = areaManager.GetWidgetInfo(path);
			// 构建编辑表单
			var widgetManager = Application.Ioc.Resolve<VisualWidgetManager>();
			var form = widgetManager.GetWidgetEditForm(widgetInfo);
			// 绑定表单
			form.BindValues(args);
			return new TemplateResult("theme.visualeditor/widget_edit_form.html", new { form });
		}

		/// <summary>
		/// 提交编辑模块的表单
		/// </summary>
		[Action("api/visual_editor/submit_widget_edit_form", HttpMethods.POST)]
		public IActionResult SubmitWidgetEditForm(string path) {
			// 获取模块信息
			var areaManager = Application.Ioc.Resolve<TemplateAreaManager>();
			var widgetInfo = areaManager.GetWidgetInfo(path);
			// 构建编辑表单
			var widgetManager = Application.Ioc.Resolve<VisualWidgetManager>();
			var form = widgetManager.GetWidgetEditForm(widgetInfo);
			// 获取提交的参数
			var result = form.ParseValues(Request.GetAllDictionary());
			return new JsonResult(new { result });
		}

		/// <summary>
		/// 获取模块的Html
		/// </summary>
		[Action("api/visual_editor/get_widget_html", HttpMethods.POST)]
		public IActionResult GetWidgetHtml(string url, string path, IDictionary<string, object> args) {
			var widgetManager = Application.Ioc.Resolve<VisualWidgetManager>();
			var widgetHtml = widgetManager.GetWidgetHtml(url, path, args);
			return new PlainResult(widgetHtml) { ContentType = "text/html" };
		}
	}
}
