using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Controllers;
using ZKWeb.Plugins.Common.Base.src.UIComponents.MenuItems;
using ZKWeb.Plugins.Common.MenuPage.src.UIComponents.MenuItems.Extensions;
using ZKWeb.Plugins.Common.MenuPage.src.UIComponents.MenuPage.Interfaces;

namespace ZKWeb.Plugins.Common.MenuPage.src.Controllers {
	/// <summary>
	/// 支持增删查改数据的菜单页面构建器
	/// 需要再次经过包装，请勿直接使用
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	public abstract class CrudMenuPageControllerBase<TEntity, TPrimaryKey> :
		CrudControllerBase<TEntity, TPrimaryKey>, IMenuPage
		where TEntity : class, IEntity<TPrimaryKey> {
		/// <summary>
		/// 所属的菜单分组
		/// </summary>
		public abstract string Group { get; }
		/// <summary>
		/// 菜单分组图标
		/// </summary>
		public abstract string GroupIconClass { get; }
		/// <summary>
		/// 菜单项图标
		/// </summary>
		public abstract string IconClass { get; }
		/// <summary>
		/// 显示此页面要求的权限列表
		/// </summary>
		public virtual string[] RequiredPrivileges { get { return ViewPrivileges; } }

		/// <summary>
		/// 初始化
		/// </summary>
		public CrudMenuPageControllerBase() {
			ExtraTemplateArguments["iconClass"] = IconClass;
		}

		/// <summary>
		/// 设置显示的菜单项
		/// </summary>
		/// <param name="groups">菜单项分组列表</param>
		public virtual void Setup(IList<MenuItemGroup> groups) {
			groups.SetupFrom(this);
		}
	}
}
