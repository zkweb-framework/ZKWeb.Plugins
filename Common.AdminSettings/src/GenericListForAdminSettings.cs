using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.AdminSettings.src.Model;
using ZKWeb.Plugins.Common.MenuPageBase.src;

namespace ZKWeb.Plugins.Common.AdminSettings.src {
	/// <summary>
	/// 这个类用于给后台设置快速添加只包含数据列表的页面
	/// 例子
	/// [ExportMany]
	/// public class ExampleList : GenericListForAdminSettings[Data, ExampleList] {
	///		public override string Group { get { return "Example Group"; } }
	///		public override string GroupIcon { get { return "fa fa-group"; } }
	///		public override string Name { get { return "Example List"; } }
	///		public override string IconClass { get { return "fa fa-example"; } }
	///		public override string Url { get { return "/admin/settings/example_list"; } }
	///		public override string Privilege { get { return "AdminSettings:ExampleList"; } }
	///		protected override IAjaxTableCallback<Data> GetTableCallback() { return new TableCallback(); }
	///		public class TableCallback : IAjaxTableCallback<Data> { /* 表格回调 */ }
	/// }
	/// </summary>
	/// <typeparam name="TData">列表中的数据类型</typeparam>
	/// <typeparam name="TPage">继承这个类的类型</typeparam>
	public abstract class GenericListForAdminSettings<TData, TPage>
		: GenericListForMenuPage<TData, TPage>, IAdminSettingsMenuProvider, IPrivilegesProvider
		where TData : class {
		/// <summary>
		/// 使用的权限
		/// </summary>
		public abstract string Privilege { get; }
		/// <summary>
		/// 默认需要拥有管理员权限
		/// </summary>
		public override UserTypes[] AllowedUserTypes { get { return UserTypesGroup.Admin; } }
		/// <summary>
		/// 默认需要拥有使用的权限
		/// </summary>
		public override string[] RequiredPrivileges { get { return new[] { Privilege }; } }
		/// <summary>
		/// 模板路径
		/// </summary>
		public override string TemplatePath { get { return "common.admin_settings/generic_list.html"; } }

		/// <summary>
		/// 获取权限列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetPrivileges() {
			yield return Privilege;
		}
	}
}
