using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Web;

namespace ZKWeb.Plugins.Common.Admin.src.Scaffolding {
	/// <summary>
	/// 简单的后台应用构建器
	/// </summary>
	/// <example>
	/// [ExportMany]
	/// public class ExampleApp : SimpleAdminAppBuilder {
	///		public override string Name { get { return "Example"; } }
	///		public override string Url { get { return "/admin/example"; } }
	///		protected override IActionResult Action() { }
	/// }
	/// </example>
	public abstract class SimpleAdminAppBuilder :
		IPrivilegesProvider, IWebsiteStartHandler, IAdminApp {
		/// <summary>
		/// 应用名称
		/// </summary>
		public abstract string Name { get; }
		/// <summary>
		/// url地址
		/// </summary>
		public abstract string Url { get; }
		/// <summary>
		/// 格式的css类名
		/// </summary>
		public virtual string TileClass { get { return "tile bg-navy"; } }
		/// <summary>
		/// 图标的css类名
		/// </summary>
		public virtual string IconClass { get { return "fa fa-archive"; } }
		/// <summary>
		/// 允许显示此应用的用户类型列表
		/// </summary>
		public virtual UserTypes[] AllowedUserTypes { get { return UserTypesGroup.Admin; } }
		/// <summary>
		/// 显示此应用要求的权限列表
		/// </summary>
		public virtual string[] RequiredPrivileges { get { return new string[0]; } }
		/// <summary>
		/// 对应的处理函数，会自动进行权限检查
		/// </summary>
		/// <returns></returns>
		protected abstract IActionResult Action();

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
