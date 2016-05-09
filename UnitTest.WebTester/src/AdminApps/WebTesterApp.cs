using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Templating;
using ZKWeb.Utils.Functions;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web.Interfaces;

namespace ZKWeb.Plugins.UnitTest.WebTester.src.AdminApps {
	/// <summary>
	/// 网页上运行单元测试
	/// 功能：
	/// - 显示测试表格
	/// - 定时从服务器抓取测试状态
	///   ?action=fetch
	/// - 清空测试结果
	///   ?action=clear
	/// - 运行指定项或全部项
	///   ?action=execute&assembly=name
	///   ?action=execute_all
	/// </summary>
	[ExportMany]
	public class WebTesterApp : AdminApp {
		public override string Name { get { return "UnitTest"; } }
		public override string Url { get { return "/admin/unit_test/web_tester"; } }
		public override string TileClass { get { return "tile bg-black"; } }
		public override string IconClass { get { return "fa fa-bug"; } }
		public override string[] RequiredPrivileges { get { return new[] { Name + ":Run" }; } }

		protected virtual IActionResult PostAction() {
			return null;
		}

		protected override IActionResult Action() {
			var request = HttpContextUtils.CurrentContext.Request;
			if (request.HttpMethod == HttpMethods.POST) {
				return PostAction();
			}
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var table = templateManager.RenderTemplate("unittest.webtester/tests_table.html", new { });
			return new TemplateResult("common.admin/generic_list.html",
				new { iconClass = IconClass, title = new T(Name), table = new HtmlString(table) });
		}
	}
}
