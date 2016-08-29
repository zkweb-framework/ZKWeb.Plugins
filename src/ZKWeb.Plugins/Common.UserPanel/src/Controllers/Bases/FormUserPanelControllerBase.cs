using ZKWeb.Plugins.Common.MenuPage.src.Controllers.Bases;
using ZKWeb.Plugins.Common.UserPanel.src.MenuPages.UIComponents.Interfaces;
using System;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;

namespace ZKWeb.Plugins.Common.UserPanel.src.Controllers.Bases {
	/// <summary>
	/// 带单个表单的用户中心页面控制器
	/// </summary>
	public abstract class FormUserPanelControllerBase :
		FormMenuPageControllerBase, IUserPanelMenuProvider {
		/// <summary>
		/// 默认需要用户登录
		/// </summary>
		public override Type RequiredUserType { get { return typeof(IAmUser); } }
		/// <summary>
		/// 默认不需要其他权限，但要做好数据隔离
		/// </summary>
		public override string[] RequiredPrivileges { get { return new string[0]; } }
		/// <summary>
		/// 模板路径
		/// </summary>
		public override string TemplatePath { get { return "common.user_panel/generic_form.html"; } }
	}
}
