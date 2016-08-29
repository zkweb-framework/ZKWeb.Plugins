using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.MenuPage.src.Controllers.Bases;
using ZKWeb.Plugins.Common.UserPanel.src.MenuPages.UIComponents.Interfaces;

namespace ZKWeb.Plugins.Common.UserPanel.src.Controllers.Bases {
	/// <summary>
	/// 支持增删查改数据的后台页面控制器
	/// </summary>
	/// <typeparam name="TEntity">数据类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	public abstract class CrudUserPanelControllerBase<TEntity, TPrimaryKey> :
		CrudMenuPageControllerBase<TEntity, TPrimaryKey>, IUserPanelMenuProvider
		where TEntity : class, IEntity<TPrimaryKey> {
		/// <summary>
		/// 默认需要用户登录
		/// </summary>
		public override Type RequiredUserType { get { return typeof(IAmUser); } }
		/// <summary>
		/// 默认不需要其他权限，但要做好数据隔离
		/// </summary>
		public override string[] RequiredPrivileges { get { return new string[0]; } }
		public override string[] ViewPrivileges { get { return new string[0]; } }
		public override string[] EditPrivileges { get { return new string[0]; } }
		public override string[] DeletePrivileges { get { return new string[0]; } }
		public override string[] DeleteForeverPrivileges { get { return new string[0]; } }
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
		/// 默认需要按实体的所属用户进行隔离
		/// </summary>
		protected override bool ConcernEntityOwnership { get { return true; } }
	}
}
