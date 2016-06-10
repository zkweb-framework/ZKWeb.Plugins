using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.MenuPage.src.Extensions;
using ZKWeb.Plugins.Common.MenuPage.src.Model;
using ZKWeb.Web;
using ZKWeb.Web.Interfaces;

namespace ZKWeb.Plugins.Common.MenuPage.src.Scaffolding {
	/// <summary>
	/// 简单的菜单页面的建器
	/// 需要再次经过包装，请勿直接使用
	/// </summary>
	public abstract class SimpleMenuPageBuilder :
		IMenuPage, IPrivilegesProvider, IWebsiteStartHandler {
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
		/// 允许显示此页面的用户类型列表
		/// </summary>
		public abstract UserTypes[] AllowedUserTypes { get; }
		/// <summary>
		/// 显示此页面要求的权限列表
		/// </summary>
		public abstract string[] RequiredPrivileges { get; }
		/// <summary>
		/// 对应的处理函数，会自动进行权限检查
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// 网站启动时注册处理函数
		/// </summary>
		public virtual void OnWebsiteStart() {
			var controllerManager = Application.Ioc.Resolve<ControllerManager>();
			var privilegesCheckedAction = new Func<IActionResult>(() => {
				var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
				privilegeManager.Check(AllowedUserTypes, RequiredPrivileges);
				return Action();
			});
			controllerManager.RegisterAction(Url, HttpMethods.GET, privilegesCheckedAction);
			controllerManager.RegisterAction(Url, HttpMethods.POST, privilegesCheckedAction);
		}
	}
}
