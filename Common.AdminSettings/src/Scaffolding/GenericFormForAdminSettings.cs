using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Web.ActionResults;
using ZKWeb.Plugins.Common.Admin.src;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.AdminSettings.src.Model;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.MenuPageBase.src;
using ZKWeb.Plugins.Common.MenuPageBase.src.Scaffolding;

namespace ZKWeb.Plugins.Common.AdminSettings.src.Scaffolding {
	/// <summary>
	/// 用于给后台设置快速添加只包含表单的页面
	/// 例子
	/// [ExportMany]
	/// public class ExampleForm : GenericFormForAdminSettings {
	///		public override string Group { get { return "Example Group"; } }
	///		public override string GroupIcon { get { return "fa fa-group"; } }
	///		public override string Name { get { return "Example Form"; } }
	///		public override string IconClass { get { return "fa fa-example"; } }
	///		public override string Url { get { return "/admin/settings/example_form"; } }
	///		public override string Privilege { get { return "AdminSettings:ExampleForm"; } }
	///		protected override IModelFormBuilder GetForm() { return new Form(); }
	///		public class Form : ModelFormBuilder { /* 表单内容 */ }
	/// }
	/// </summary>
	public abstract class GenericFormForAdminSettings :
		GenericFormForMenuPage, IAdminSettingsMenuProvider {
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
