using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Model;
using ZKWeb.Model.ActionResults;
using ZKWeb.Plugins.Common.Admin.src;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.MenuPageBase.src {
	/// <summary>
	/// 用于快速添加菜单页中的页面
	/// 这个抽象类需要再次继承，请勿直接使用
	/// 目前集成这个类的类有GenericFormForMenuPage, GenericListForMenuPage
	/// </summary>
	public abstract class GenericPageForMenuPage {
		/// <summary>
		/// 所属分组
		/// </summary>
		public abstract string Group { get; }
		/// <summary>
		/// 分组图标，只有分组不存在时才会使用这里的图标
		/// </summary>
		public abstract string GroupIcon { get; }
		/// <summary>
		/// 页面名称
		/// </summary>
		public abstract string Name { get; }
		/// <summary>
		/// 图标的Css类
		/// </summary>
		public abstract string IconClass { get; }
		/// <summary>
		/// Url地址
		/// </summary>
		public abstract string Url { get; }
		/// <summary>
		/// 要求的用户类型
		/// </summary>
		public abstract UserTypes[] AllowedUserTypes { get; }
		/// <summary>
		/// 要求的权限
		/// </summary>
		public abstract string[] RequiredPrivileges { get; }
		/// <summary>
		/// 模板路径
		/// </summary>
		public abstract string TemplatePath { get; }

		/// <summary>
		/// 设置显示的菜单项
		/// </summary>
		/// <param name="groups">菜单项分组列表</param>
		public virtual void Setup(List<MenuItemGroup> groups) {
			// 没有权限时不显示菜单项
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			if (user == null || !AllowedUserTypes.Contains(user.Type) ||
				!PrivilegesChecker.HasPrivileges(user, RequiredPrivileges)) {
				return;
			}
			// 添加菜单项
			var group = groups.FirstOrDefault(g => g.Name == Group);
			if (group == null) {
				group = new MenuItemGroup(Group, GroupIcon);
				groups.Add(group);
			}
			group.Items.AddItemForLink(new T(Name), IconClass, Url);
		}

		/// <summary>
		/// 请求的处理函数
		/// </summary>
		protected virtual IActionResult Action() {
			// 检查权限
			PrivilegesChecker.Check(AllowedUserTypes, RequiredPrivileges);
			// 返回模板页
			return new TemplateResult(TemplatePath);
		}

		/// <summary>
		/// 网站启动时添加处理函数
		/// </summary>
		public virtual void OnWebsiteStart() {
			var controllerManager = Application.Ioc.Resolve<ControllerManager>();
			controllerManager.RegisterAction(Url, HttpMethods.GET, Action);
			controllerManager.RegisterAction(Url, HttpMethods.POST, Action);
		}
	}
}
