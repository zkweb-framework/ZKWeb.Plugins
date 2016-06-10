using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Web.ActionResults;
using ZKWeb.Plugins.Common.Admin.src;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.MenuPageBase.src;
using ZKWeb.Plugins.Common.MenuPageBase.src.Scaffolding;
using ZKWeb.Plugins.Common.UserPanel.src.Model;

namespace ZKWeb.Plugins.Common.UserPanel.src.Scaffolding {
	/// <summary>
	/// 用于给用户快速添加只包含表单的页面
	/// 例子
	/// [ExportMany]
	/// public class ExampleForm : GenericFormForUserPanel {
	///		public override string Group { get { return "Example Group"; } }
	///		public override string GroupIcon { get { return "fa fa-group"; } }
	///		public override string Name { get { return "Example Form"; } }
	///		public override string IconClass { get { return "fa fa-example"; } }
	///		public override string Url { get { return "/home/example_form"; } }
	///		public override IModelFormBuilder GetForm() { return new Form(); }
	///		public class Form : ModelFormBuilder { /* 表单内容 */ }
	/// }
	/// </summary>
	public abstract class GenericFormForUserPanel : GenericFormForMenuPage, IUserPanelMenuProvider {
		/// <summary>
		/// 默认要求用户登录
		/// </summary>
		public override UserTypes[] AllowedUserTypes { get { return UserTypesGroup.All; } }
		/// <summary>
		/// 默认不要求具体权限
		/// </summary>
		public override string[] RequiredPrivileges { get { return new string[0]; } }
		/// <summary>
		/// 模板路径
		/// </summary>
		public override string TemplatePath { get { return "common.user_panel/generic_form.html"; } }
	}
}
