using System;
using System.Collections.Generic;
using System.FastReflection;
using System.Linq;
using System.Reflection;
using ZKWeb.Plugins.Common.Admin.src.Components.ScaffoldAttributes;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Web;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src.Controllers.Bases {
	/// <summary>
	/// 用于自动生成页面的控制器的基类
	/// Url和需要的权限可以从控制器的成员得到
	/// </summary>
	public class ScaffoldControllerBase : ControllerBase, IWebsiteStartHandler {
		/// <summary>
		/// 注册这个控制器中的Action函数
		/// </summary>
		/// <param name="controllerManager">控制器管理器</param>
		/// <param name="action">Action函数</param>
		/// <param name="actionFilterAttributes">Action过滤器属性</param>
		/// <param name="scaffoldActionAttributes">ScaffoldAction属性，Url由成员获取</param>
		/// <param name="scaffoldActionFilterAttributes">ScaffoldActionFilter属性，实际的过滤器根据成员生成</param>
		protected virtual void RegisterScaffoldAction(
			ControllerManager controllerManager,
			Func<IActionResult> action,
			IEnumerable<ActionFilterAttribute> actionFilterAttributes,
			IEnumerable<ScaffoldActionAttribute> scaffoldActionAttributes,
			IEnumerable<ScaffoldActionFilterAttribute> scaffoldActionFilterAttributes) {
			try {
				// 应用Action过滤器
				foreach (var actionFilter in actionFilterAttributes) {
					action = actionFilter.Filter(action);
				}
				// 应用ScaffoldAction过滤器
				foreach (var scaffoldActionFilter in scaffoldActionFilterAttributes) {
					action = scaffoldActionFilter.GetFilter(this).Filter(action);
				}
				// 注册Action函数
				foreach (var scaffoldAction in scaffoldActionAttributes) {
					var path = scaffoldAction.GetPath(this);
					var method = scaffoldAction.HttpMethod;
					if (!string.IsNullOrEmpty(path)) {
						controllerManager.RegisterAction(path, method, action);
					}
				}
			} catch (Exception e) {
				// 注册失败时抛出例外，并显示注册失败的控制器类型和Url属性名
				throw new InvalidOperationException(string.Format(
					"Register scaffold action {0} in {1} failed",
					string.Join(",", scaffoldActionAttributes.Select(a => a.PathProperty)),
					GetType().FullName), e);
			}
		}

		/// <summary>
		/// 网站启动时注册这个类中带ScaffoldAction属性的函数
		/// </summary>
		public virtual void OnWebsiteStart() {
			var type = GetType();
			var controllerManager = Application.Ioc.Resolve<ControllerManager>();
			var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			foreach (var method in type.FastGetMethods(bindingFlags)) {
				// 忽略getter和setter函数
				if (method.IsSpecialName) {
					continue;
				}
				// 获取Action属性和ScaffoldAction属性
				var actionAttributes = method.GetAttributes<ActionAttribute>(true);
				var scaffoldActionAttributes = method.GetAttributes<ScaffoldActionAttribute>(true);
				if (!actionAttributes.Any() && !scaffoldActionAttributes.Any()) {
					continue;
				}
				// 获取ActionFilter属性和ScaffoldActionFilter属性
				var actionFilterAttributes = method.GetAttributes<ActionFilterAttribute>(true);
				var scaffoldActionFilterAttributes = method.GetAttributes<ScaffoldActionFilterAttribute>(true);
				// 检查是否同时使用了Action属性和ScaffoldActionFilter属性
				// 如果同时使用了则抛出错误，因为ScaffoldActionFilter属性不会起作用
				if (actionAttributes.Any() && scaffoldActionFilterAttributes.Any()) {
					throw new InvalidOperationException(
						"Please don't mixed ActionAttribute and ScaffoldActionFilterAttribute, " +
						"ScaffoldActionFilter won't work, you can use ScaffoldActionAttribute instead");
				}
				// 构建Action函数
				var action = this.BuildActionDelegate(method);
				// 注册Action函数
				RegisterScaffoldAction(
					controllerManager,
					action,
					actionFilterAttributes,
					scaffoldActionAttributes,
					scaffoldActionFilterAttributes);
			}
		}
	}
}
