using ZKWeb.Plugins.Common.Admin.src.Components.ActionFilters;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
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
			return new TemplateResult("theme.visualeditor/top-bar.html");
		}
	}
}
