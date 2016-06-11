using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.MenuPage.src.Scaffolding;
using ZKWeb.Plugins.Common.UserPanel.src.Model;

namespace ZKWeb.Plugins.Common.UserPanel.src.Scaffolding {
	/// <summary>
	/// 支持增删查改数据的后台页面构建器
	/// <example>
	/// public class ExamplePage : UserPanelCrudPageBuilder[ExampleTable] {
	///		public override string Group { get { return "Example Group"; } }
	///		public override string GroupIconClass { get { return "fa fa-group"; } }
	///		public override string Name { get { return "Example Page"; } }
	///		public override string IconClass { get { return "fa fa-example"; } }
	///		public override string Url { get { return "/user/example_page"; } }
	///		protected override IModelFormBuilder GetAddForm() { return new Form(); }
	///		protected override IModelFormBuilder GetEditForm() { return new Form(); }
	///		protected override IAjaxTableCallback[ExampleTable] GetTableCallback() { return new TableCallback(); }
	///		public class TableCallback : IAjaxTableCallback[ExampleTable] { }
	///		public class Form : UserOwnedDataEditFormBuilder[ExampleTable, Form] { }
	/// }
	/// </example>
	/// </summary>
	/// <typeparam name="TData">数据类型</typeparam>
	public abstract class UserPanelCrudPageBuilder<TData> :
		CrudMenuPageBuilder<TData>, IUserPanelMenuProvider
		where TData : class {
		/// <summary>
		/// 默认需要用户登录
		/// </summary>
		public override UserTypes[] AllowedUserTypes { get { return UserTypesGroup.All; } }
		/// <summary>
		/// 默认不需要特别的权限，但需要做好数据隔离
		/// </summary>
		public override string[] RequiredPrivileges { get { return new string[0]; } }
		public override string[] ViewPrivileges { get { return new string[0]; } }
		public override string[] EditPrivileges { get { return new string[0]; } }
		public override string[] DeletePrivileges { get { return new string[0]; } }
		public override string[] DeleteForeverPrivilege { get { return new string[0]; } }
		/// <summary>
		/// 列表页的模板路径
		/// </summary>
		public override string ListTemplatePath { get { return "common.user_panel/generic_list.html"; } }
		/// <summary>
		/// 添加页的模板路径
		/// </summary>
		public override string AddTemplatePath { get { return "common.user_panel/generic_add.html"; } }
		/// <summary>
		/// 编辑页的模板路径
		/// </summary>
		public override string EditTemplatePath { get { return "common.user_panel/generic_edit.html"; } }

		/// <summary>
		/// 获取批量操作的数据Id列表
		/// 并检查当前登录用户是否有数据的所有权
		/// </summary>
		/// <returns></returns>
		protected override IList<object> GetBatchActionIds() {
			var ids = base.GetBatchActionIds();
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.CheckOwnership<TData>(ids);
			return ids;
		}
	}
}
