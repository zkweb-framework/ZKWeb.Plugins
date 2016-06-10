using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.MenuPageBase.src.Scaffolding;
using ZKWeb.Plugins.Common.UserPanel.src.Model;

namespace ZKWeb.Plugins.Common.UserPanel.src.Scaffolding {
	/// <summary>
	/// 用于列出数据的用户中心页面构建器
	/// <example>
	/// TODO: 编写这里的例子
	/// </example>
	/// </summary>
	/// <typeparam name="TData">数据类型</typeparam>
	public abstract class GenericListForUserPanel<TData> :
		GenericListForMenuPage<TData>, IUserPanelMenuProvider
		where TData : class {
		/// <summary>
		/// 默认需要管理员权限
		/// </summary>
		public override UserTypes[] AllowedUserTypes { get { return UserTypesGroup.All; } }
		/// <summary>
		/// 默认需要使用的权限
		/// </summary>
		public override string[] RequiredPrivileges { get { return new string[0]; } }
		/// <summary>
		/// 模板路径
		/// </summary>
		public override string TemplatePath { get { return "common.user_panel/generic_list.html"; } }
	}
}
