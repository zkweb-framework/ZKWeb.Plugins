using System;
using System.FastReflection;
using System.Reflection;
using ZKWeb.Plugins.Common.Admin.src.Components.ActionFilters;
using ZKWeb.Web;

namespace ZKWeb.Plugins.Common.Admin.src.Components.ScaffoldAttributes {
	/// <summary>
	/// ScaffoldController中使用的CheckOwner属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ScaffoldCheckOwnerAttribute : ScaffoldActionFilterAttribute {
		/// <summary>
		/// 保存了是否启用CheckOwner的成员
		/// 成员类型应该是bool
		/// </summary>
		public string EnableProperty { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="enableProperty">保存了是否启用CheckOwner的成员</param>
		public ScaffoldCheckOwnerAttribute(string enableProperty) {
			EnableProperty = enableProperty;
		}

		/// <summary>
		/// 获取实际的Action过滤器
		/// </summary>
		public override IActionFilter GetFilter(IController controller) {
			var controllerType = controller.GetType();
			var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			var enableProperty = controllerType.FastGetProperty(EnableProperty, bindingFlags);
			var enabled = (bool)enableProperty.FastGetValue(controller);
			if (enabled) {
				return new CheckOwnerAttribute();
			} else {
				return new TransparentActionFilter();
			}
		}
	}
}
