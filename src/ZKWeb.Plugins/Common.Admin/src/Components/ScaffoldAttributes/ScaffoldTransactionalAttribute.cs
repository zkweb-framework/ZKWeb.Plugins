using System;
using System.Data;
using System.FastReflection;
using System.Reflection;
using ZKWeb.Plugins.Common.Admin.src.Components.ActionFilters;
using ZKWeb.Web;

namespace ZKWeb.Plugins.Common.Admin.src.Components.ScaffoldAttributes {
	/// <summary>
	/// ScaffoldController中使用的Transactional属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ScaffoldTransactionalAttribute : ScaffoldActionFilterAttribute {
		/// <summary>
		/// 保存了是否启用Transactional的成员
		/// 成员类型应该是bool
		/// </summary>
		public string EnableProperty { get; set; }
		/// <summary>
		/// 保存了隔离等级的成员
		/// 成员类型应该是IsolationLevel?
		/// </summary>
		public string IsolationLevelProperty { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="enableProperty">保存了是否启用Transactional的成员</param>
		/// <param name="enableProperty">保存了隔离等级的成员</param>
		public ScaffoldTransactionalAttribute(
			string enableProperty, string isolationLevelProperty) {
			EnableProperty = enableProperty;
			IsolationLevelProperty = isolationLevelProperty;
		}

		/// <summary>
		/// 获取实际的Action过滤器
		/// </summary>
		public override IActionFilter GetFilter(IController controller) {
			var controllerType = controller.GetType();
			var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			var enableProperty = controllerType.FastGetProperty(EnableProperty, bindingFlags);
			var isolationLevelProperty = controllerType.FastGetProperty(IsolationLevelProperty, bindingFlags);
			var enabled = (bool)enableProperty.FastGetValue(controller);
			if (enabled) {
				var isolationLevel = (IsolationLevel?)isolationLevelProperty.FastGetValue(controller);
				return new TransactionalAttribute(isolationLevel);
			} else {
				return new TransparentActionFilter();
			}
		}
	}
}
