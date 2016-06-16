using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.AdminSettings.src.Model;
using ZKWeb.Plugins.Common.MenuPage.src.Scaffolding;

namespace ZKWeb.Plugins.Common.AdminSettings.src.Scaffolding {
	/// <summary>
	/// 带单个表单的后台设置页面构建器
	/// <example>
	/// [ExportMany]
	/// public class ExamplePage : AdminSettingsFormPageBuilder {
	///		public override string Group { get { return "Example Group"; } }
	///		public override string GroupIconClass { get { return "fa fa-group"; } }
	///		public override string Name { get { return "Example Page"; } }
	///		public override string IconClass { get { return "fa fa-example"; } }
	///		public override string Url { get { return "/admin/settings/example_page"; } }
	///		public override string Privilege { get { return "AdminSettings:ExamplePage"; } }
	///		protected override IModelFormBuilder GetForm() { return new Form(); }
	///		public class Form : ModelFormBuilder { }
	/// }
	/// </example>
	/// </summary>
	public abstract class AdminSettingsFormPageBuilder :
		FormMenuPageBuilder, IAdminSettingsMenuProvider {
		/// <summary>
		/// 使用的权限
		/// </summary>
		public abstract string Privilege { get; }
		/// <summary>
		/// 默认需要管理员权限
		/// </summary>
		public override UserTypes[] AllowedUserTypes { get { return UserTypesGroup.Admin; } }
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
