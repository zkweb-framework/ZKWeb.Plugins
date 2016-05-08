using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web.Interfaces;

namespace ZKWeb.Plugins.UnitTest.WebTester.src.AdminApps {
	/// <summary>
	/// 网页上运行单元测试
	/// 功能：
	/// - 显示测试表格
	/// - 定时从服务器抓取测试状态
	/// - 清空测试结果
	/// - 运行指定项或全部项
	/// </summary>
	[ExportMany]
	public class WebTesterApp : AdminApp, IPrivilegesProvider {
		public override string Name { get { return "UnitTest"; } }
		public override string Url { get { return "/admin/unit_test/web_tester"; } }
		public override string TileClass { get { return "tile bg-black"; } }
		public override string IconClass { get { return "fa fa-bug"; } }
		public virtual string Privilege { get { return Name + ":Execute"; } }

		public IEnumerable<string> GetPrivileges() {
			yield return Privilege;
		}

		protected override IActionResult Action() {
			PrivilegesChecker.Check(UserTypesGroup.Admin, Privilege);
			return new PlainResult("asdasdas");
		}
	}
}
