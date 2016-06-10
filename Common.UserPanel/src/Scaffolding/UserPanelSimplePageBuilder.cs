using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.MenuPage.src.Scaffolding;
using ZKWeb.Plugins.Common.UserPanel.src.Model;

namespace ZKWeb.Plugins.Common.UserPanel.src.Scaffolding {
	/// <summary>
	/// 简单的用户中心页面构建器
	/// <example>
	/// [ExportMany]
	/// public class ExamplePage : UserPanelSimplePageBuilder {
	///		public override string Group { get { return "Example Group"; } }
	///		public override string GroupIconClass { get { return "fa fa-group"; } }
	///		public override string Name { get { return "Example Page"; } }
	///		public override string IconClass { get { return "fa fa-example"; } }
	///		public override string Url { get { return "/user/example_page"; } }
	///		protected override IActionResult Action() { return new PlainResult("example page"); }
	/// }
	/// </example>
	/// </summary>
	public abstract class UserPanelSimplePageBuilder :
		SimpleMenuPageBuilder, IUserPanelMenuProvider {
		/// <summary>
		/// 默认需要用户登录
		/// </summary>
		public override UserTypes[] AllowedUserTypes { get { return UserTypesGroup.All; } }
		/// <summary>
		/// 默认不需要特别的权限，但需要做好数据隔离
		/// </summary>
		public override string[] RequiredPrivileges { get { return new string[0]; } }
	}
}
