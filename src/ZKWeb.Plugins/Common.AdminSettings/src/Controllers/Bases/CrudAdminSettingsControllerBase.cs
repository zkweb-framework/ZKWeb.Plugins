using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.AdminSettings.src.UIComponents.MenuPages.Interfaces;
using ZKWeb.Plugins.Common.MenuPage.src.Controllers.Bases;

namespace ZKWeb.Plugins.Common.AdminSettings.src.Controllers.Bases {
	/// <summary>
	/// 支持增删查改数据的后台页面控制器
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	public abstract class CrudAdminSettingsControllerBase<TEntity, TPrimaryKey> :
		CrudMenuPageControllerBase<TEntity, TPrimaryKey>, IAdminSettingsMenuProvider
		where TEntity : class, IEntity<TPrimaryKey> {
		/// <summary>
		/// 默认需要管理员权限
		/// </summary>
		public override Type RequiredUserType { get { return typeof(IAmAdmin); } }
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
