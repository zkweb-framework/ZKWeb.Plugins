using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.AdminSettings.src.Model;
using ZKWeb.Plugins.Common.MenuPage.src.Scaffolding;

namespace ZKWeb.Plugins.Common.AdminSettings.src.Scaffolding {
	/// <summary>
	/// 支持增删查改数据的后台页面构建器
	/// </summary>
	/// <example>
	/// public class ExamplePage : AdminSettingsCrudPageBuilder[ExampleTable] {
	///		public override string Group { get { return "Example Group"; } }
	///		public override string GroupIconClass { get { return "fa fa-group"; } }
	///		public override string Name { get { return "Example Page"; } }
	///		public override string IconClass { get { return "fa fa-example"; } }
	///		public override string Url { get { return "/admin/settings/example_page"; } }
	///		protected override IModelFormBuilder GetAddForm() { return new Form(); }
	///		protected override IModelFormBuilder GetEditForm() { return new Form(); }
	///		protected override IAjaxTableCallback[ExampleTable] GetTableCallback() { return new TableCallback(); }
	///		public class TableCallback : IAjaxTableCallback[ExampleTable] { }
	///		public class Form : DataEditFormBuilder[ExampleTable, Form] { }
	/// }
	/// </example>
	/// <typeparam name="TData">数据类型</typeparam>
	public abstract class AdminSettingsCrudPageBuilder<TData> :
		CrudMenuPageBuilder<TData>, IAdminSettingsMenuProvider
		where TData : class, IEntity {
		/// <summary>
		/// 默认需要管理员权限
		/// </summary>
		public override UserTypes[] AllowedUserTypes { get { return UserTypesGroup.Admin; } }
		/// <summary>
		/// 列表页的模板路径
		/// </summary>
		public override string ListTemplatePath { get { return "common.admin_settings/generic_list.html"; } }
		/// <summary>
		/// 添加页的模板路径
		/// </summary>
		public override string AddTemplatePath { get { return "common.admin_settings/generic_add.html"; } }
		/// <summary>
		/// 编辑页的模板路径
		/// </summary>
		public override string EditTemplatePath { get { return "common.admin_settings/generic_edit.html"; } }
	}
}
