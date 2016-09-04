using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.Controllers.Extensions;
using ZKWeb.Plugins.Testing.WebTester.src.Domain.Services;
using ZKWeb.Templating;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Testing.WebTester.src.Controllers {
	/// <summary>
	/// 网页上运行单元测试
	/// 功能：
	/// - 显示测试表格
	/// - 定时向服务器抓取测试信息
	///   ?action=fetch
	/// - 重置测试结果
	///   ?action=reset_all
	/// - 运行指定项或全部项
	///   ?action=start＆assembly=name
	///   ?action=start_all
	/// </summary>
	[ExportMany]
	public class WebTesterController : SimpleAdminAppControllerBase {
		public override string Group { get { return "Function Test"; } }
		public override string GroupIconClass { get { return "fa fa-flask"; } }
		public override string Name { get { return "Run tests"; } }
		public override string Url { get { return "/admin/unit_test/web_tester"; } }
		public override string TileClass { get { return "tile bg-black"; } }
		public override string IconClass { get { return "fa fa-flask"; } }
		public override string[] RequiredPrivileges { get { return new[] { Name + ":Run" }; } }

		/// <summary>
		/// 处理POST请求
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult PostAction() {
			this.RequireAjaxRequest();
			var request = Request;
			var action = request.Get<string>("action");
			var webTesterManager = Application.Ioc.Resolve<WebTesterManager>();
			if (action == "fetch") {
				// 抓取测试信息
				var updateTimes = request.Get<Dictionary<string, string>>("updateTimes") ??
					new Dictionary<string, string>();
				var informations = webTesterManager.GetInformations(updateTimes);
				return new JsonResult(new { informations });
			} else if (action == "reset_all") {
				// 重置测试结果
				webTesterManager.ResetInformations();
				return new JsonResult(new { message = new T("Request submitted, wait processing") });
			} else if (action == "start_all") {
				// 开始全部测试
				webTesterManager.RunAllAssemblyTestBackground();
				return new JsonResult(new { message = new T("Request submitted, wait processing") });
			} else if (action == "start") {
				// 开始单项测试
				var assemblyName = request.Get<string>("assembly");
				webTesterManager.RunAssemblyTestBackground(assemblyName);
				return new JsonResult(new { message = new T("Request submitted, wait processing") });
			}
			throw new ArgumentException(string.Format("unknown action {0}", action));
		}

		/// <summary>
		/// 处理请求
		/// </summary>
		/// <returns></returns>
		protected override IActionResult Action() {
			// 处理POST请求
			if (Request.Method == HttpMethods.POST) {
				return PostAction();
			}
			// 显示测试表格
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var webTesterManager = Application.Ioc.Resolve<WebTesterManager>();
			var assemblyNames = webTesterManager.GetInformations()
				.Select(info => info.AssemblyName).ToList();
			var table = templateManager.RenderTemplate(
				"testing.webtester/tests_table.html", new { assemblyNames });
			return new TemplateResult("common.admin/generic_list.html", new {
				title = new T(Name),
				table = new HtmlString(table),
				includeJs = new[] { "/static/testing.webtester.js/webtester.min.js" },
				includeCss = new[] { "/static/testing.webtester.css/webtester.css" },
				extra = new { iconClass = IconClass }
			});
		}
	}
}
