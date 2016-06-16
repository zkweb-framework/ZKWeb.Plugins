using ZKWeb.Plugins.Common.Admin.src.Model;

namespace ZKWeb.Plugins.Common.Admin.src.Scaffolding {
	/// <summary>
	/// 支持增删查改数据的后台应用构建器
	/// <example>
	///	[ExportMany]
	///	public class ExampleApp : AdminAppBuilder[TestData] {
	///		public override string Name { get { return "ExampleManager"; } }
	///		public override string Url { get { return "/admin/example"; } }
	///		protected override IAjaxTableCallback<TestData> GetTableCallback() { return new TableCallback(); }
	///		protected override FormBuilder GetAddForm() { return new Form(); }
	///		protected override FormBuilder GetEditForm() { return new Form(); }
	///		public class TableCallback : IAjaxTableCallback[TestData] { }
	///		public class Form : DataEditFormBuilder[TestData, Form] { }
	/// }
	/// </example>
	/// </summary>
	/// <typeparam name="TData">数据类型</typeparam>
	public abstract class AdminAppBuilder<TData> : CrudPageBuilder<TData>, IAdminApp
		where TData : class {
		/// <summary>
		/// 格子的css类名
		/// </summary>
		public virtual string TileClass { get { return "tile bg-navy"; } }
		/// <summary>
		/// 图标的css类名
		/// </summary>
		public virtual string IconClass { get { return "fa fa-archive"; } }
		/// <summary>
		/// 默认需要管理员权限
		/// </summary>
		public override UserTypes[] AllowedUserTypes { get { return UserTypesGroup.Admin; } }
		/// <summary>
		/// 默认需要查看权限
		/// </summary>
		public virtual string[] RequiredPrivileges { get { return ViewPrivileges; } }
		/// <summary>
		/// 列表页的模板路径
		/// </summary>
		public override string ListTemplatePath { get { return "common.admin/generic_list.html"; } }
		/// <summary>
		/// 添加页的模板路径
		/// </summary>
		public override string AddTemplatePath { get { return "common.admin/generic_add.html"; } }
		/// <summary>
		/// 编辑页的模板路径
		/// </summary>
		public override string EditTemplatePath { get { return "common.admin/generic_edit.html"; } }

		/// <summary>
		/// 初始化
		/// </summary>
		public AdminAppBuilder() : base() {
			ExtraTemplateArguments["iconClass"] = IconClass;
		}
	}
}
