using System;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Controllers.ActionResults;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Controllers {
	/// <summary>
	/// 基础Api控制器
	/// </summary>
	[ExportMany]
	public class BaseApiController : ControllerBase {
		/// <summary>
		/// 导出Ajax表格的数据
		/// </summary>
		/// <returns></returns>
		[Action("/api/base/export_ajax_table", HttpMethods.POST)]
		public IActionResult ExportAjaxTable() {
			// 获取Ajax表格的搜索结果
			// 这里会同时检查权限等，和直接搜索时的流程相同
			var target = Request.Get<string>("target");
			var controllerManager = Application.Ioc.Resolve<ControllerManager>();
			var searchAction = controllerManager.GetAction(target, HttpMethods.POST);
			if (searchAction == null) {
				throw new BadRequestException($"Search action {target} not found");
			}
			var response = (searchAction() as JsonResult)?.Object as AjaxTableSearchResponse;
			if (response == null) {
				throw new BadRequestException($"Parse search result from {target} failed");
			}
			// 把Ajax表格转换到静态表格，并返回导出结果
			var staticTable = response.ToTableBuilder();
			var filenameWithoutExtensions = string.Join("_",
				target.Substring(1).Replace('/', '_').ToUpper() +
				DateTime.UtcNow.ToClientTime().ToString("yyyyMMdd_HHmmss"));
			return new StaticTableExportResult(staticTable, filenameWithoutExtensions);
		}
	}
}
