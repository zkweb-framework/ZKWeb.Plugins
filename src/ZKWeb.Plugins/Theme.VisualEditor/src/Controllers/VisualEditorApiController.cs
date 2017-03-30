using System;
using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Components.ActionFilters;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ScriptStrings;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Services;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs;
using ZKWeb.Plugins.Theme.VisualEditor.src.UIComponents.Forms;
using ZKWeb.Templating.DynamicContents;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

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
		/// 获取管理主题的表单
		/// </summary>
		/// <returns></returns>
		[Action("api/visual_editor/get_manage_theme_form")]
		[CheckPrivilege(typeof(ICanUseAdminPanel), "VisualEditor:VisualEditor")]
		public IActionResult GetManageThemeForm() {
			var themeManager = Application.Ioc.Resolve<VisualThemeManager>();
			var themes = themeManager.GetThemes();
			var backupThemes = themeManager.GetBackupThemes();
			var uploadThemeForm = new UploadThemeForm();
			var createThemeForm = new CreateThemeForm();
			uploadThemeForm.Bind();
			createThemeForm.Bind();
			return new TemplateResult("theme.visualeditor/manage_theme.html",
				new { themes, backupThemes, uploadThemeForm, createThemeForm });
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
		/// 下载主题
		/// </summary>
		/// <returns></returns>
		[Action("api/visual_editor/download_theme")]
		[CheckPrivilege(typeof(ICanUseAdminPanel), "VisualEditor:VisualEditor")]
		public IActionResult DownloadTheme(string filename) {
			var themeManager = Application.Ioc.Resolve<VisualThemeManager>();
			var stream = themeManager.GetThemeStream(filename);
			Response.AddHeader("Content-Disposition",
				$"attachment; filename={HttpUtils.UrlEncode(filename)}");
			return new StreamResult(stream, "application/zip");
		}

		/// <summary>
		/// 下载备份主题
		/// </summary>
		/// <returns></returns>
		[Action("api/visual_editor/download_backup_theme")]
		[CheckPrivilege(typeof(ICanUseAdminPanel), "VisualEditor:VisualEditor")]
		public IActionResult DownloadBackupTheme(string filename) {
			var themeManager = Application.Ioc.Resolve<VisualThemeManager>();
			var stream = themeManager.GetBackupThemeStream(filename);
			Response.AddHeader("Content-Disposition",
				$"attachment; filename={HttpUtils.UrlEncode(filename)}");
			return new StreamResult(stream, "application/zip");
		}

		/// <summary>
		/// 应用主题
		/// </summary>
		/// <returns></returns>
		[Action("api/visual_editor/apply_theme", HttpMethods.POST)]
		[CheckPrivilege(typeof(ICanUseAdminPanel), "VisualEditor:VisualEditor")]
		public IActionResult ApplyTheme(string filename) {
			var themeManager = Application.Ioc.Resolve<VisualThemeManager>();
			themeManager.ApplyTheme(filename);
			return new JsonResult(new {
				message = new T("Apply theme success, reloading this page......"),
				script = BaseScriptStrings.RefreshAfter(3000)
			});
		}

		/// <summary>
		/// 应用备份主题
		/// </summary>
		/// <returns></returns>
		[Action("api/visual_editor/apply_backup_theme", HttpMethods.POST)]
		[CheckPrivilege(typeof(ICanUseAdminPanel), "VisualEditor:VisualEditor")]
		public IActionResult ApplyBackupTheme(string filename) {
			var themeManager = Application.Ioc.Resolve<VisualThemeManager>();
			themeManager.ApplyBackupTheme(filename);
			return new JsonResult(new {
				message = new T("Apply theme success, reloading this page......"),
				script = BaseScriptStrings.RefreshAfter(3000)
			});
		}

		/// <summary>
		/// 删除主题
		/// </summary>
		/// <returns></returns>
		[Action("api/visual_editor/delete_theme", HttpMethods.POST)]
		[CheckPrivilege(typeof(ICanUseAdminPanel), "VisualEditor:VisualEditor")]
		public IActionResult DeleteTheme(string filename) {
			var themeManager = Application.Ioc.Resolve<VisualThemeManager>();
			themeManager.DeleteTheme(filename);
			return new JsonResult(new {
				message = new T("Delete theme success"),
				script = BaseScriptStrings.RefreshModal
			});
		}

		/// <summary>
		/// 创建主题
		/// </summary>
		/// <returns></returns>
		[Action("api/visual_editor/create_theme", HttpMethods.POST)]
		[CheckPrivilege(typeof(ICanUseAdminPanel), "VisualEditor:VisualEditor")]
		public IActionResult CreateTheme() {
			var form = new CreateThemeForm();
			return new JsonResult(form.Submit());
		}

		/// <summary>
		/// 上传主题
		/// </summary>
		/// <returns></returns>
		[Action("api/visual_editor/upload_theme", HttpMethods.POST)]
		[CheckPrivilege(typeof(ICanUseAdminPanel), "VisualEditor:VisualEditor")]
		public IActionResult UploadTheme() {
			var form = new UploadThemeForm();
			return new JsonResult(form.Submit());
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

		/// <summary>
		/// 保存可视化编辑的修改
		/// </summary>
		[Action("api/visual_editor/save_changes", HttpMethods.POST)]
		public IActionResult SaveChanges(VisualEditResult result) {
			var themeManager = Application.Ioc.Resolve<VisualThemeManager>();
			themeManager.SaveEditResult(result);
			return new JsonResult(new { message = new T("Saved Successfully") });
		}
	}
}
