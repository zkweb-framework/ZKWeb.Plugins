using ZKWeb.Plugins.Common.Admin.src.Components.ActionFilters;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Services;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Ioc;

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
			return new TemplateResult("theme.visualeditor/top_bar.html");
		}

		/// <summary>
		/// 获取切换页面弹出框的Html
		/// </summary>
		/// <returns></returns>
		[Action("api/visual_editor/get_switch_pages")]
		[CheckPrivilege(typeof(ICanUseAdminPanel), "VisualEditor:VisualEditor")]
		public IActionResult GetSwitchPages() {
			var pageManager = Application.Ioc.Resolve<VisualPageManager>();
			var groups = pageManager.GetEditablePages();
			return new TemplateResult("theme.visualeditor/switch_pages.html", new { groups });
		}
	}
}
