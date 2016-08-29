using System;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.AdminSettings.src.UIComponents.MenuPages.Interfaces;
using ZKWeb.Plugins.Common.MenuPage.src.Controllers.Bases;

namespace ZKWeb.Plugins.Common.AdminSettings.src.Controllers.Bases {
	/// <summary>
	/// 带单个表单的后台设置页面控制器
	/// </summary
	public abstract class FormAdminSettingsControllerBase :
		FormMenuPageControllerBase, IAdminSettingsMenuProvider {
		/// <summary>
		/// 使用的权限
		/// </summary>
		public abstract string Privilege { get; }
		/// <summary>
		/// 默认需要管理员权限
		/// </summary>
		public override Type RequiredUserType { get { return typeof(IAmAdmin); } }
		/// <summary>
		/// 默认需要使用的权限
		/// </summary>
		public override string[] RequiredPrivileges { get { return new[] { Privilege }; } }
		/// <summary>
		/// 模板路径
		/// </summary>
		public override string TemplatePath { get { return "common.admin_settings/generic_form.html"; } }
	}
}
