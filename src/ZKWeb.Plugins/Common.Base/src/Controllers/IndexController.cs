using System;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Controllers {
	/// <summary>
	/// 首页控制器
	/// </summary>
	[ExportMany]
	public class IndexController : IController {
		/// <summary>
		/// 首页
		/// </summary>
		/// <returns></returns>
		[Action("/")]
		public IActionResult Index() {
			return new TemplateResult("common.base/index.html");
		}
	}
}
