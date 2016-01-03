using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Model;

namespace ZKWeb.Plugins.Common.Admin.src.AdminApps {
	/// <summary>
	/// 测试应用
	/// </summary>
	[ExportMany]
	public class TestAppC : AdminApp {
		public override string Name { get { return "TestAppC"; } }
		public override string Url { get { return "/admin/app_c"; } }
	}
}
