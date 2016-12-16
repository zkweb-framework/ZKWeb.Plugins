using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Admin.src.Components.PrivilegeProviders;
using ZKWeb.Plugins.Common.Admin.src.Components.ScaffoldAttributes;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.MenuItems;
using ZKWeb.Plugins.Common.MenuPage.src.UIComponents.MenuItems.Extensions;
using ZKWeb.Plugins.Common.MenuPage.src.UIComponents.MenuPages.Interfaces;
using ZKWeb.Web;

namespace ZKWeb.Plugins.Common.MenuPage.src.Controllers.Bases {
	/// <summary>
	/// 简单的菜单页面控制器的基础类
	/// 需要再次经过包装，请勿直接使用
	/// </summary>
	public abstract class SimpleMenuPageControllerBase :
		ScaffoldControllerBase,
		IMenuPage, IPrivilegesProvider {
		/// <summary>
		/// 所属的菜单分组
		/// </summary>
		public abstract string Group { get; }
		/// <summary>
		/// 菜单分组图标
		/// </summary>
		public abstract string GroupIconClass { get; }
		/// <summary>
		/// 菜单项名称
		/// </summary>
		public abstract string Name { get; }
		/// <summary>
		/// 菜单项图标
		/// </summary>
		public abstract string IconClass { get; }
		/// <summary>
		/// Url地址
		/// </summary>
		public abstract string Url { get; }
		/// <summary>
		/// 允许显示此页面的用户类型
		/// </summary>
		public abstract Type RequiredUserType { get; }
		/// <summary>
		/// 显示此页面要求的权限列表
		/// </summary>
		public abstract string[] RequiredPrivileges { get; }
		/// <summary>
		/// 对应的处理函数，会自动进行权限检查
		/// </summary>
		/// <returns></returns>
		[ScaffoldAction(nameof(Url), HttpMethods.GET)]
		[ScaffoldAction(nameof(Url), HttpMethods.POST)]
		[ScaffoldCheckPrivilege(nameof(RequiredUserType), nameof(RequiredPrivileges))]
		protected abstract IActionResult Action();

		/// <summary>
		/// 设置显示的菜单项
		/// </summary>
		/// <param name="groups">菜单项分组列表</param>
		public virtual void Setup(IList<MenuItemGroup> groups) {
			groups.SetupFrom(this);
		}

		/// <summary>
		/// 获取权限列表
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<string> GetPrivileges() {
			foreach (var privilege in RequiredPrivileges) {
				yield return privilege;
			}
		}
	}
}
