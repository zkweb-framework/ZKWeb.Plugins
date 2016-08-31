using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;

namespace ZKWeb.Plugins.Common.Admin.src.Controllers.Bases {
	/// <summary>
	/// 增删查改使用的后台控制器的基础类
	/// </summary>
	public abstract class CrudAdminAppControllerBase<TEntity, TPrimaryKey> :
		CrudControllerBase<TEntity, TPrimaryKey>, IAdminAppController
		where TEntity : class, IEntity<TPrimaryKey> {
		/// <summary>
		/// 分组名称
		/// </summary>
		public virtual string Group { get { return "Other"; } }
		/// <summary>
		/// 分组图标的css类
		/// </summary>
		public virtual string GroupIconClass { get { return "fa fa-archive"; } }
		/// <summary>
		/// 格子的css类
		/// </summary>
		public virtual string TileClass { get { return "tile bg-navy"; } }
		/// <summary>
		/// 图标的css类
		/// </summary>
		public virtual string IconClass { get { return "fa fa-archive"; } }
		/// <summary>
		/// 默认需要管理员权限
		/// </summary>
		public override Type RequiredUserType { get { return typeof(IAmAdmin); } }
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
		/// 管理员操作数据默认不考虑所属用户
		/// </summary>
		protected override bool ConcernEntityOwnership { get { return false; } }

		/// <summary>
		/// 初始化
		/// </summary>
		public CrudAdminAppControllerBase() : base() {
			ExtraTemplateArguments["iconClass"] = IconClass;
		}
	}
}
